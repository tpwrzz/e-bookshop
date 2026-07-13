using Basket.Domain.Repositories;
using Bookshop.Contracts.Grpc;

namespace Basket.Infrastructure.Grpc;

public class CatalogPriceClient(CatalogGrpcService.CatalogGrpcServiceClient client) : ICatalogPriceClient
{
    public async Task<BookPriceInfo> GetPriceAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        var reply = await client.GetBookPriceAsync(
            new BookPriceRequest { BookId = bookId.ToString() },
            cancellationToken: cancellationToken);

        return new BookPriceInfo(bookId, reply.Found, reply.Title, (decimal)reply.Amount, reply.Currency);
    }

    public async Task<IReadOnlyList<BookPriceInfo>> GetPricesAsync(IEnumerable<Guid> bookIds, CancellationToken cancellationToken = default)
    {
        var request = new BookPricesRequest();
        request.BookIds.AddRange(bookIds.Select(id => id.ToString()));

        var reply = await client.GetBookPricesAsync(request, cancellationToken: cancellationToken);

        return reply.Prices
            .Select(p => new BookPriceInfo(Guid.Parse(p.BookId), p.Found, p.Title, (decimal)p.Amount, p.Currency))
            .ToList();
    }
}