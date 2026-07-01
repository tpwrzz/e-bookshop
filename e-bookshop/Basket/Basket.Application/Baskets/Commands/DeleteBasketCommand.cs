using Basket.Domain.Repositories;
using Bookshop.SharedKernel.Application.Common;
using MediatR;

namespace Basket.Application.Baskets.Commands;

public record DeleteBasketCommand(Guid UserId) : IRequest<Result>;

public class DeleteBasketCommandHandler(IBasketRepository repository): IRequestHandler<DeleteBasketCommand, Result>
{
    public async Task<Result> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await repository.GetAsync(request.UserId);

        if (basket is null)
            return new Result
            {
                ResultStatus = ResultStatus.NotFound,
                Message = $"Basket for user {request.UserId} not found."
            };

        await repository.DeleteAsync(request.UserId);

        return new Result
        {
            ResultStatus = ResultStatus.Success,
            Message = $"Basket for user {request.UserId} deleted."
        };
    }
}