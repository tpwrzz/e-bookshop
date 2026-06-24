using Bookshop.SharedKernel.Application.Common;
using Bookshop.SharedKernel.Domain;
using MediatR;
using Ordering.Application.DTOs;
using Ordering.Domain;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Orders.Commands
{
    public record PlaceOrderCommand(AddOrderDto Order) : IRequest<Result<Guid>>;

    public class PlaceOrderCommandHandler(IOrderRepository repository) : IRequestHandler<PlaceOrderCommand, Result<Guid>>
    {
        private readonly IOrderRepository _repository = repository;
        public async Task<Result<Guid>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var list = new List<OrderItem>();
            foreach (var item in request.Order.OrderItems)
            {
                list.Add(new OrderItem(
                    id: Guid.NewGuid(),
                    title: item.Title,
                    price: new Money(amount: item.Price, currency: request.Order.Currency),
                    quantity: item.Amount
                    ));
            }
            var order = Order.Place(
                id: Guid.NewGuid(),
                items: list,
                address: new Address(
                    request.Order.Address.Street,
                    request.Order.Address.City,
                    request.Order.Address.Country,
                    request.Order.Address.Postcode),
                totalCost: new Money(amount: list.Sum(i => i.Price.Amount * i.Quantity), currency: request.Order.Currency)
                );
            await _repository.SaveAsync(order);
            return new Result<Guid>()
            {
                ResultStatus = ResultStatus.Created,
                Data = order.Id,
                Message = $"Order with id: {order.Id} is placed"
            };
        }
    }
}
