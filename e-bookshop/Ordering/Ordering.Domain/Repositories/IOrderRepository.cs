namespace Ordering.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<(IEnumerable<Order> orders, int totalCount)> GetPagedByUserAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<Order?> GetByIdAsync(Guid id);
        Task SaveAsync(Order order);
        Task UpdateAsync(Order order);
    }
}
