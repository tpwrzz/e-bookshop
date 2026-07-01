using Basket.Domain.Repositories;
using Bookshop.Contracts.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Basket.Infrastructure.Consumers;

public class OrderPlacedConsumer(
    IBasketRepository repository,
    ILogger<OrderPlacedConsumer> logger) : IConsumer<OrderPlaced>
{
    public async Task Consume(ConsumeContext<OrderPlaced> context)
    {
        var userId = context.Message.UserId;
        logger.LogInformation("Clearing basket for user {UserId} after order {OrderId} placed",
            userId, context.Message.OrderId);

        var basket = await repository.GetAsync(userId);
        if (basket is null)
        {
            logger.LogWarning("Basket for user {UserId} not found — already cleared?", userId);
            return;
        }

        await repository.DeleteAsync(userId);
        logger.LogInformation("Basket cleared for user {UserId}", userId);
    }
}