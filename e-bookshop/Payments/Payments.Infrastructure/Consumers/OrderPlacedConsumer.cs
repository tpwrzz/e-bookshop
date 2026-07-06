using Bookshop.Contracts.Messages;
using BookShop.Contracts.Messages;
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

        // Idempotency guard
        var alreadyProcessed = await context.ProcessedMessages
            .AnyAsync(m => m.MessageId == messageId);

        if (alreadyProcessed)
        {
            logger.LogWarning("Message {MessageId} already processed — skipping", messageId);
            return;
        }

        var message = consumeContext.Message;
        logger.LogInformation("Processing payment for order {OrderId}", message.OrderId);

        var payment = Payment.Process(message.OrderId, message.UserId, message.TotalAmount);
        await repository.AddAsync(payment);

        // Mark as processed
        context.ProcessedMessages.Add(new ProcessedMessage
        {
            MessageId = messageId,
            MessageType = nameof(OrderPlaced),
            ProcessedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        // Publish result
        if (payment.Succeeded)
        {
            await publishEndpoint.Publish(new PaymentProcessed(
                message.OrderId,
                message.UserId,
                DateTime.UtcNow));

            logger.LogInformation("Payment succeeded for order {OrderId}", message.OrderId);
        }
        else
        {
            await publishEndpoint.Publish(new PaymentFailed(
                message.OrderId,
                message.UserId,
                payment.FailureReason!,
                DateTime.UtcNow));

            logger.LogWarning("Payment failed for order {OrderId}: {Reason}",
                message.OrderId, payment.FailureReason);
        }
    }
}