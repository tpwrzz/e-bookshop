using Basket.Domain;
using Basket.Domain.Repositories;
using Couchbase.Extensions.DependencyInjection;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository(INamedBucketProvider bucketProvider) : IBasketRepository
{
    private const string KeyPrefix = "basket::";

    public async Task<CustomerBasket?> GetAsync(Guid userId)
    {
        var bucket = await bucketProvider.GetBucketAsync();
        var collection = await bucket.DefaultCollectionAsync();

        try
        {
            var result = await collection.GetAsync($"{KeyPrefix}{userId}");
            return result.ContentAs<CustomerBasket>();
        }
        catch (Couchbase.Core.Exceptions.KeyValue.DocumentNotFoundException)
        {
            return null;
        }
    }

    public async Task UpsertAsync(CustomerBasket basket)
    {
        var bucket = await bucketProvider.GetBucketAsync();
        var collection = await bucket.DefaultCollectionAsync();
        await collection.UpsertAsync($"{KeyPrefix}{basket.UserId}", basket);
    }

    public async Task DeleteAsync(Guid userId)
    {
        var bucket = await bucketProvider.GetBucketAsync();
        var collection = await bucket.DefaultCollectionAsync();
        await collection.RemoveAsync($"{KeyPrefix}{userId}");
    }
}