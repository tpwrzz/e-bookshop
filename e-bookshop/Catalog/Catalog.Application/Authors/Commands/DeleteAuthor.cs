using Catalog.Application.Common;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Authors.Commands
{
    public record DeleteAuthorCommand(Guid Id) : IRequest<Result>;
    public class DeleteAuthorCommandHandler(IAuthorRepository authorRepository) : IRequestHandler<DeleteAuthorCommand, Result>
    {
        private readonly IAuthorRepository _repository = authorRepository;
        public async Task<Result> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _repository.GetByIdAsync(request.Id);
            if (author is null)
                return new Result
                {
                    ResultStatus = ResultStatus.NotFound,
                    Message = $"Author with Id {request.Id} was not found."
                };

            await _repository.DeleteAsync(request.Id);
            return new Result
            {
                ResultStatus = ResultStatus.Success,
                Message = $"Author with Id {request.Id} was deleted."
            };
        }
    }
}

