using BookShop.Contracts.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Enums;
using Ordering.Infrastructure.Idempotency;

namespace Ordering.Infrastructure.Consumers;

public class PaymentProcessedConsumer(
    OrderingContext context,
    ILogger<PaymentProcessedConsumer> logger) : IConsumer<PaymentProcessed>
{
    public async Task Consume(ConsumeContext<PaymentProcessed> consumeContext)
    {
        var messageId = consumeContext.MessageId ?? Guid.NewGuid();

        var alreadyProcessed = await context.ProcessedMessages
            .AnyAsync(m => m.MessageId == messageId);

        if (alreadyProcessed)
        {
            logger.LogWarning("Message {MessageId} already processed — skipping", messageId);
            return;
        }

        var message = consumeContext.Message;
        var order = await context.Orders.FindAsync(message.OrderId);

        if (order is null)
        {
            logger.LogWarning("Order {OrderId} not found", message.OrderId);
            return;
        }

        try
        {
            order.TransitionStatus(OrderStatus.Confirmed);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning("Could not confirm order {OrderId}: {Reason}", message.OrderId, ex.Message);
            return;
        }

        context.ProcessedMessages.Add(new ProcessedMessage
        {
            MessageId = messageId,
            MessageType = nameof(PaymentProcessed),
            ProcessedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
        logger.LogInformation("Order {OrderId} confirmed after payment", message.OrderId);
    }
}