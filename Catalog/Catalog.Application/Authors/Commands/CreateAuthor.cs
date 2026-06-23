using Catalog.Application.Common;
using Catalog.Application.DTOs.Auhtors;
using Catalog.Domain;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Authors.Commands
{
    public record CreateAuthorCommand(AuthorDto Author) : IRequest<Result>;
    public class CreateAuthorCommandHandler(IAuthorRepository authorRepository) : IRequestHandler<CreateAuthorCommand, Result>
    {
        private readonly IAuthorRepository _repository = authorRepository;
        public async Task<Result> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author(id: Guid.NewGuid(), firstName: request.Author.FirstName, lastName: request.Author.LastName, bio: request.Author.Bio);

            await _repository.AddAsync(author);

            return new Result()
            {
                ResultStatus = ResultStatus.Created,
                Message = $"New Author was added with Id: {author.Id}"
            };
        }
    }
}
