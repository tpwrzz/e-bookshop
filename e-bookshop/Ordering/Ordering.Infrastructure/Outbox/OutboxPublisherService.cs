using Bookshop.Contracts.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ordering.Infrastructure.Outbox;

public class OutboxPublisherService(
    IServiceScopeFactory scopeFactory,
    ILogger<OutboxPublisherService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PublishPendingMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error publishing outbox messages");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    private async Task PublishPendingMessagesAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<OrderingContext>();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        var messages = await context.OutboxMessages
            .Where(m => m.ProcessedAt == null)
            .OrderBy(m => m.CreatedAt)
            .Take(20)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            if (message.Type == nameof(OrderPlaced))
            {
                var payload = JsonSerializer.Deserialize<OrderPlaced>(message.Payload);
                if (payload is not null)
                    await publishEndpoint.Publish(payload, cancellationToken);
            }

            message.ProcessedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}