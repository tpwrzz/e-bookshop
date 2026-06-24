using Bookshop.SharedKernel.Application.Common;
using MediatR;
using Ordering.Application.DTOs;
using Ordering.Domain;
using Ordering.Domain.Enums;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Orders.Commands
{
    public record PlaceOrderCommand(OrderDto Order) : IRequest<Result>;

    public class PlaceOrderCommandHandler(IOrderRepository repository) : IRequestHandler<PlaceOrderCommand, Result>
    {
        private readonly IOrderRepository _repository = repository;
        public async Task<Result> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            var list = new List<OrderItem>();
            foreach (var item in request.Order.OrderItems)
            {
                list.Add(new OrderItem(
                    id: Guid.NewGuid(),
                    title: item.Title,
                    price: new Bookshop.SharedKernel.Domain.Money(amount: item.Price, currency: request.Order.Currency),
                    amount: item.Amount
                    ));
            }
            var address = request.Order.Address.Split(',');
            var order = new Order(
                id: Guid.NewGuid(),
                orderItems: list,
                orderStatus: OrderStatus.Pending,
                address: new Address(address[0].Trim(), address[1].Trim(), address[2].Trim(), address[3].Trim()),
                totalCost: new Bookshop.SharedKernel.Domain.Money(amount: request.Order.TotalCost, currency: request.Order.Currency)
                );
            await _repository.SaveAsync(order);
            return new Result()
            {
                ResultStatus = ResultStatus.Success,
                Message = $"Order with id: {request.Order.Id} is placed"
            };
        }
    }
}
