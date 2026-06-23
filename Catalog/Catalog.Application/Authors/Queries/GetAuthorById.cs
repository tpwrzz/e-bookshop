using Catalog.Application.Common;
using Catalog.Application.DTOs.Auhtors;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Authors.Queries
{
    public record GetAuthorByIdCommand(Guid Id) : IRequest<Result<AuthorDto>>;
    public class GetAuthorByIdCommandHandler(IAuthorRepository authorRepository) : IRequestHandler<GetAuthorByIdCommand, Result<AuthorDto>>
    {
        private readonly IAuthorRepository _repository = authorRepository;
        public async Task<Result<AuthorDto>> Handle(GetAuthorByIdCommand request, CancellationToken cancellationToken)
        {
            var author = await _repository.GetByIdAsync(request.Id);
            if (author is null)
                return new Result<AuthorDto>
                {
                    ResultStatus = ResultStatus.NotFound,
                    Data = null,
                    Message = $"Author with Id {request.Id} was not found."
                };

            var dto = new AuthorDto() { FirstName = author.FirstName, Id = author.Id, LastName = author.LastName, Bio = author.Bio };
            return new Result<AuthorDto>
            {
                ResultStatus = ResultStatus.Success,
                Data = dto,
                Message = $"Author with Id {request.Id} was deleted."
            };
        }
    }
}
