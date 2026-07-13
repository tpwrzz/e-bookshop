using Basket.Application.Baskets.Commands;
using Basket.Application.Baskets.Queries;
using Basket.Application.DTOs;
using Bookshop.SharedKernel.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/basket")]
public class BasketsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBasket(Guid userId)
    {
        var result = await mediator.Send(new GetBasketQuery(userId));

        return result.ResultStatus switch
        {
            ResultStatus.Success => Ok(result.Data),
            ResultStatus.NotFound => NotFound(result.Message),
            _ => StatusCode(500, result.Message)
        };
    }

    [HttpPut("upsert")]
    public async Task<IActionResult> UpsertBasket(BasketDto basket)
    {
        var result = await mediator.Send(new UpsertBasketCommand(basket));

        return result.ResultStatus switch
        {
            ResultStatus.Success => Ok(result.Message),
            _ => StatusCode(500, result.Message)
        };
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteBasket(Guid userId)
    {
        var result = await mediator.Send(new DeleteBasketCommand(userId));

        return result.ResultStatus switch
        {
            ResultStatus.Success => Ok(result.Message),
            ResultStatus.NotFound => NotFound(result.Message),
            _ => StatusCode(500, result.Message)
        };
    }

    [HttpGet("price/{bookId}")]
    public async Task<IActionResult> GetBookPrice(Guid bookId)
    {
        var result = await mediator.Send(new GetBookPriceQuery(bookId));
        return result.ResultStatus switch
        {
            ResultStatus.Success => Ok(result.Data),
            ResultStatus.NotFound => NotFound(result.Message),
            _ => StatusCode(500, result.Message)
        };
    }
}