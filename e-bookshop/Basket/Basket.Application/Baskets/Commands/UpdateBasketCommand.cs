using Basket.Application.DTOs;
using Basket.Domain;
using Basket.Domain.Repositories;
using Bookshop.SharedKernel.Application.Common;
using MediatR;

namespace Basket.Application.Baskets.Commands;

public record UpsertBasketCommand(BasketDto Basket) : IRequest<Result>;

public class UpsertBasketCommandHandler(
    IBasketRepository repository,
    ICatalogPriceClient priceClient) : IRequestHandler<UpsertBasketCommand, Result>
{
    public async Task<Result> Handle(UpsertBasketCommand request, CancellationToken cancellationToken)
    {
        var bookIds = request.Basket.Items.Select(i => i.BookId).Distinct().ToList();
        var prices = await priceClient.GetPricesAsync(bookIds, cancellationToken);
        var priceLookup = prices.ToDictionary(p => p.BookId);

        var items = new List<BasketItem>();
        foreach (var item in request.Basket.Items)
        {
            if (!priceLookup.TryGetValue(item.BookId, out var priceInfo) || !priceInfo.Found)
            {
                return new Result
                {
                    ResultStatus = ResultStatus.NotFound,
                    Message = $"Book with Id {item.BookId} was not found in Catalog."
                };
            }

            items.Add(new BasketItem
            {
                BookId = item.BookId,
                Title = priceInfo.Title,
                UnitPrice = priceInfo.Amount, 
                Quantity = item.Quantity
            });
        }

        var basket = new CustomerBasket { UserId = request.Basket.UserId, Items = items };
        await repository.UpsertAsync(basket);

        return new Result
        {
            ResultStatus = ResultStatus.Success,
            Message = $"Basket for user {request.Basket.UserId} updated."
        };
    }
}