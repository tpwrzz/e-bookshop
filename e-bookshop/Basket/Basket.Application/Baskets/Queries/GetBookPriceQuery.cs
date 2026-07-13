using Basket.Domain.Repositories;
using Bookshop.SharedKernel.Application.Common;
using MediatR;

namespace Basket.Application.Baskets.Queries;

public record GetBookPriceQuery(Guid BookId) : IRequest<Result<BookPriceInfo>>;

public class GetBookPriceQueryHandler(ICatalogPriceClient priceClient)
    : IRequestHandler<GetBookPriceQuery, Result<BookPriceInfo>>
{
    public async Task<Result<BookPriceInfo>> Handle(GetBookPriceQuery request, CancellationToken cancellationToken)
    {
        var price = await priceClient.GetPriceAsync(request.BookId, cancellationToken);

        if (!price.Found)
            return new Result<BookPriceInfo> { ResultStatus = ResultStatus.NotFound, Data = null!, Message = $"Book {request.BookId} not found." };

        return new Result<BookPriceInfo> { ResultStatus = ResultStatus.Success, Data = price, Message = string.Empty };
    }
}