namespace Basket.Domain.Repositories;

public interface IBasketRepository
{
    Task<CustomerBasket?> GetAsync(Guid userId);
    Task UpsertAsync(CustomerBasket basket);
    Task DeleteAsync(Guid userId);
}