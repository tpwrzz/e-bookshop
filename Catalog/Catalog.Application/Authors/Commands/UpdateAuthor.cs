using Catalog.Application.Common;
using Catalog.Application.DTOs.Auhtors;
using Catalog.Domain;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Authors.Commands
{
    public record UpdateAuthorCommand(AuthorDto Author) : IRequest<Result>;
    public class UpdateAuthorCommandHandler(IAuthorRepository authorRepository) : IRequestHandler<UpdateAuthorCommand, Result>
    {
        private readonly IAuthorRepository _repository = authorRepository;
        public async Task<Result> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var authorToChange = await _repository.GetByIdAsync(request.Author.Id);
            if (authorToChange != null)
            {
                return new Result()
                {
                    ResultStatus = ResultStatus.NotFound,
                    Message = $"Author with Id: {request.Author.Id} not found"
                };
            }
            var author = new Author(id: request.Author.Id, firstName: request.Author.FirstName, lastName: request.Author.LastName, bio: request.Author.Bio);
            await _repository.UpdateAsync(author);

            return new Result()
            {
                ResultStatus = ResultStatus.Created,
                Message = $"New Author was added with Id: {author.Id}"
            };
        }
    }
}
