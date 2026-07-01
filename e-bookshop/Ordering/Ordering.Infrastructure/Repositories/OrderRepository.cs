using Bookshop.Contracts.Messages;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain;
using Ordering.Domain.Repositories;
using Ordering.Infrastructure.Outbox;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository(OrderingContext context) : IOrderRepository
    {
        private readonly OrderingContext _context = context;
        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<(IEnumerable<Order> orders, int totalCount)> GetPagedByUserAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.Orders.Where(o => o.UserId == userId);
            var orders = await query
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .ToListAsync(cancellationToken);
            var totalCount = await query.CountAsync(cancellationToken);
            return (orders, totalCount);
        }

        public async Task SaveAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            foreach (var domainEvent in order.DomainEvents)
            {
                if (domainEvent is OrderPlacedEvent placedEvent)
                {
                    var payload = JsonSerializer.Serialize(new OrderPlaced(
                        OrderId: placedEvent.Id,
                        UserId: order.UserId,
                        TotalAmount: placedEvent.TotalCost.Amount,
                        Currency: placedEvent.TotalCost.Currency.ToString(),
                        PlacedAt: DateTime.UtcNow
                    ));

                    _context.OutboxMessages.Add(new OutboxMessage
                    {
                        Id = Guid.NewGuid(),
                        Type = nameof(OrderPlaced),
                        Payload = payload,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
            await _context.SaveChangesAsync();
            order.ClearDomainEvents();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
