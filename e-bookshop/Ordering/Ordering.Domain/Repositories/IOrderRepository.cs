namespace Ordering.Domain.Repositories
{
    public interface IOrderRepository
    {
        IQueryable<Order> GetAll();
        Task<Order> GetByIdAsync(Guid id);
        Task SaveAsync(Order order);
        Task UpdateAsync(Order order);
    }
}
