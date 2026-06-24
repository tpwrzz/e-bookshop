using Bookshop.SharedKernel.Application.Common;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Books.Commands
{
    public record DeleteBookCommand(Guid Id) : IRequest<Result>;

    public class DeleteBookCommandHandler(IBookRepository repository): IRequestHandler<DeleteBookCommand, Result>
    {
        private readonly IBookRepository _repository = repository;

        public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _repository.GetByIdAsync(request.Id);
            if (book is null)
                return new Result
                {
                    ResultStatus = ResultStatus.NotFound,
                    Message = $"Book with Id {request.Id} was not found."
                };

            await _repository.DeleteAsync(request.Id);
            return new Result
            {
                ResultStatus = ResultStatus.Success,
                Message = $"Book with Id {request.Id} was deleted."
            };
        }
    }
}
