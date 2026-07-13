namespace Basket.Domain.Repositories;

public record BookPriceInfo(Guid BookId, bool Found, string Title, decimal Amount, string Currency);

public interface ICatalogPriceClient
{
    Task<BookPriceInfo> GetPriceAsync(Guid bookId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BookPriceInfo>> GetPricesAsync(IEnumerable<Guid> bookIds, CancellationToken cancellationToken = default);
}