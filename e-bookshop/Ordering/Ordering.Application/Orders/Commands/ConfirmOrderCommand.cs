using Bookshop.SharedKernel.Application.Common;
using MediatR;
using Ordering.Domain.Enums;
using Ordering.Domain.Repositories;

namespace Ordering.Application.Orders.Commands
{
    public record ConfirmOrderCommand(Guid Id) : IRequest<Result>;

    public class ConfirmOrderCommandHandler(IOrderRepository repository) : IRequestHandler<ConfirmOrderCommand, Result>
    {
        private readonly IOrderRepository _repository = repository;
        public async Task<Result> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repository.GetByIdAsync(request.Id);
            if (order is null) return new Result()
            {
                Message = $"Order with id: {request.Id} now found",
                ResultStatus = ResultStatus.NotFound
            };
            order.UpdateOrderStatus(OrderStatus.Confirmed);
            await _repository.UpdateAsync(order);
            return new Result()
            {
                ResultStatus = ResultStatus.Success,
                Message = $"Order with id: {request.Id} is confirmed"
            };
        }
    }
}
