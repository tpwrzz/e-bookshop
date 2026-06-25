using Bookshop.SharedKernel.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.DTOs;
using Ordering.Application.Orders.Commands;
using Ordering.Application.Orders.Queries;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(id));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Data),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId(Guid id)
        {
            var result = await _mediator.Send(new GetOrdersByUserQuery(id));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Data),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }
        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder(AddOrderDto order)
        {
            var result = await _mediator.Send(new PlaceOrderCommand(order));

            return result.ResultStatus switch
            {
                ResultStatus.Created => CreatedAtAction(nameof(OrderDto), result.Message),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }
        [HttpPatch("cancel/{id}")]
        public async Task<IActionResult> CancelOrder(Guid id)
        {
            var result = await _mediator.Send(new CancelOrderCommand(id));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Message),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }
        [HttpPatch("confirm/{id}")]
        public async Task<IActionResult> ConfirmOrder(Guid id)
        {
            var result = await _mediator.Send(new ConfirmOrderCommand(id));

            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result.Message),
                ResultStatus.NotFound => NotFound(result.Message),
                _ => StatusCode(500, result.Message)
            };
        }

    }
}
