using Bookshop.Contracts.Messages;
using Bookshop.Contracts.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payments.Domain;
using Payments.Domain.Repositories;
using Payments.Infrastructure.Idempotency;

namespace Payments.Infrastructure.Consumers;

public class OrderPlacedConsumer(
    IPaymentRepository repository,
    PaymentsContext context,
    IPublishEndpoint publishEndpoint,
    ILogger<OrderPlacedConsumer> logger) : IConsumer<OrderPlaced>
{
    public async Task Consume(ConsumeContext<OrderPlaced> consumeContext)
    {
        var messageId = consumeContext.MessageId ?? Guid.NewGuid();

        var alreadyProcessed = await context.ProcessedMessages
            .AnyAsync(m => m.MessageId == messageId);

        if (alreadyProcessed)
        {
            logger.LogWarning("Message {MessageId} for order {OrderId} already processed - skipping",
                messageId, consumeContext.Message.OrderId);
            return;
        }

        var message = consumeContext.Message;
        logger.LogInformation("Processing payment for order {OrderId}, amount {Amount} {Currency}",
                              message.OrderId,
                              message.TotalAmount,
                              message.Currency);

        var payment = Payment.Process(message.OrderId, message.UserId, message.TotalAmount);
        await repository.AddAsync(payment);

        context.ProcessedMessages.Add(new ProcessedMessage
        {
            MessageId = messageId,
            MessageType = nameof(OrderPlaced),
            ProcessedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        if (payment.Succeeded)
        {
            await publishEndpoint.Publish(new PaymentProcessed(
                message.OrderId,
                message.UserId,
                DateTime.UtcNow));

            logger.LogInformation("Payment {PaymentId} succeeded for order {OrderId}, amount {Amount}", payment.Id, message.OrderId, message.TotalAmount);
        }
        else
        {
            await publishEndpoint.Publish(new PaymentFailed(
                message.OrderId,
                message.UserId,
                payment.FailureReason!,
                DateTime.UtcNow));

            logger.LogWarning("Payment {PaymentId} failed for order {OrderId}: {Reason}", payment.Id, message.OrderId, payment.FailureReason);
        }
    }
}