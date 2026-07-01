using Basket.Application.DTOs;
using Basket.Domain;
using Basket.Domain.Repositories;
using Bookshop.SharedKernel.Application.Common;
using MediatR;

namespace Basket.Application.Baskets.Queries;

public record GetBasketQuery(Guid UserId) : IRequest<Result<ShowCustomerBasketDto>>;

public class GetBasketQueryHandler(IBasketRepository repository): IRequestHandler<GetBasketQuery, Result<ShowCustomerBasketDto>>
{
    public async Task<Result<ShowCustomerBasketDto>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await repository.GetAsync(request.UserId);

        if (basket is null)
            return new Result<ShowCustomerBasketDto>
            {
                ResultStatus = ResultStatus.NotFound,
                Data = null!,
                Message = $"Basket for user {request.UserId} not found."
            };

        return new Result<ShowCustomerBasketDto>
        {
            ResultStatus = ResultStatus.Success,
            Data = MapToDto(basket),
            Message = string.Empty
        };
    }

    private static ShowCustomerBasketDto MapToDto(CustomerBasket basket) => new()
    {
        UserId = basket.UserId,
        TotalPrice = basket.TotalPrice,
        Items = basket.Items.Select(i => new BasketItemDto
        {
            BookId = i.BookId,
            Title = i.Title,
            UnitPrice = i.UnitPrice,
            Quantity = i.Quantity
        }).ToList()
    };
}