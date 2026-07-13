using Bookshop.Contracts.Grpc;
using Catalog.Domain.Repositories;
using Grpc.Core;

namespace Catalog.API.Services;

public class CatalogGrpcServiceImpl(IBookRepository bookRepository)
    : CatalogGrpcService.CatalogGrpcServiceBase
{
    public override async Task<BookPriceReply> GetBookPrice(BookPriceRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.BookId, out var bookId))
            return new BookPriceReply { BookId = request.BookId, Found = false };

        var book = await bookRepository.GetByIdAsync(bookId);
        if (book is null)
            return new BookPriceReply { BookId = request.BookId, Found = false };

        return new BookPriceReply
        {
            BookId = book.Id.ToString(),
            Found = true,
            Title = book.Title,
            Amount = (double)book.Price.Amount,
            Currency = book.Price.Currency.ToString()
        };
    }

    public override async Task<BookPricesReply> GetBookPrices(BookPricesRequest request, ServerCallContext context)
    {
        var reply = new BookPricesReply();

        foreach (var idStr in request.BookIds)
        {
            if (!Guid.TryParse(idStr, out var bookId))
            {
                reply.Prices.Add(new BookPriceReply { BookId = idStr, Found = false });
                continue;
            }

            var book = await bookRepository.GetByIdAsync(bookId);

            reply.Prices.Add(book is null
                ? new BookPriceReply { BookId = idStr, Found = false }
                : new BookPriceReply
                {
                    BookId = book.Id.ToString(),
                    Found = true,
                    Title = book.Title,
                    Amount = (double)book.Price.Amount,
                    Currency = book.Price.Currency.ToString()
                });
        }

        return reply;
    }
}