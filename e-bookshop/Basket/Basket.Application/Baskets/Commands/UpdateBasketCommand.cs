using Basket.Application.DTOs;
using Basket.Domain;
using Basket.Domain.Repositories;
using Bookshop.SharedKernel.Application.Common;
using MediatR;

namespace Basket.Application.Baskets.Commands;

public record UpsertBasketCommand(BasketDto Basket) : IRequest<Result>;

public class UpsertBasketCommandHandler(IBasketRepository repository): IRequestHandler<UpsertBasketCommand, Result>
{
    public async Task<Result> Handle(UpsertBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = new CustomerBasket
        {
            UserId = request.Basket.UserId,
            Items = request.Basket.Items.Select(i => new BasketItem
            {
                BookId = i.BookId,
                Title = i.Title,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity
            }).ToList()
        };

        await repository.UpsertAsync(basket);

        return new Result
        {
            ResultStatus = ResultStatus.Success,
            Message = $"Basket for user {request.Basket.UserId} updated."
        };
    }
}