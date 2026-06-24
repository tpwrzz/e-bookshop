using Bookshop.SharedKernel.Application.Common;
using MediatR;
using Ordering.Application.DTOs;
using Ordering.Domain;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Orders.Queries
{
    public record GetOrdersByUserQuery(Guid UserId, int Page = 1, int PageSize = 10) : IRequest<Result<PagedResult<OrderDto>>>;
    public class GetOrdersByUserQueryHandler(IOrderRepository repository) : IRequestHandler<GetOrdersByUserQuery, Result<PagedResult<OrderDto>>>
    {
        private readonly IOrderRepository _repository = repository;
        public async Task<Result<PagedResult<OrderDto>>> Handle(GetOrdersByUserQuery request, CancellationToken cancellationToken)
        {
            var (orders, totalCount) = await _repository.GetPagedByUserAsync(request.UserId, request.Page, request.PageSize, cancellationToken);
            var dtos = orders.Select(b => MapToDto(b)).ToList();

            var pagedResult = new PagedResult<OrderDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return new Result<PagedResult<OrderDto>>
            {
                ResultStatus = ResultStatus.Success,
                Data = pagedResult,
                Message = string.Empty
            };
        }

        private OrderDto MapToDto(Order b)
        {
            var list = new List<OrderItemDto>();
            foreach (var item in b.OrderItems)
            {
                list.Add(new OrderItemDto() { Id = item.Id, Price = item.Price.Amount, Title = item.Title, Quantity = item.Quantity });
            }
            return new OrderDto()
            {
                Id = b.Id,
                OrderItems = list,
                Address = new AddressDto() { City = b.Address.City, Street = b.Address.Street, Country = b.Address.Country, Postcode=b.Address.Postcode},
                Currency = b.TotalCost.Currency,
                TotalCost = b.TotalCost.Amount,
                OrderStatus = b.OrderStatus,
                PlacedDate = b.PlacedDate,
                UserId = b.UserId
            };
        }
    }
}
