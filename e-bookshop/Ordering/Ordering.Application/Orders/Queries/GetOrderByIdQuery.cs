using Bookshop.SharedKernel.Application.Common;
using MediatR;
using Ordering.Application.DTOs;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Orders.Queries
{
    public record GetOrderByIdQuery(Guid Id) : IRequest<Result<OrderDto>>;

    public class GetOrderByIdQueryHandler(IOrderRepository repository) : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
    {
        private readonly IOrderRepository _repository = repository;
        public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.Id);
            if (order is null)
            {
                return new Result<OrderDto>()
                {
                    Data = null,
                    Message = $"Order with id: {request.Id} not found",
                    ResultStatus = ResultStatus.NotFound
                };
            }
            var list = new List<OrderItemDto>();
            foreach (var item in order.OrderItems)
            {
                list.Add(new OrderItemDto()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Price = item.Price.Amount,
                    Quantity = item.Quantity
                });
            }
            var dto = new OrderDto()
            {
                Id = request.Id,
                OrderItems = list,
                Address = new AddressDto() { Street = order.Address.Street, City = order.Address.City, Country = order.Address.Country, Postcode = order.Address.Postcode},
                OrderStatus = order.OrderStatus,
                TotalCost = order.TotalCost.Amount,
                Currency = order.TotalCost.Currency
            };
            return new Result<OrderDto>()
            {
                Data = dto,
                Message = $"Order with id: {request.Id} was found",
                ResultStatus = ResultStatus.Success
            };
        }
    }
}
