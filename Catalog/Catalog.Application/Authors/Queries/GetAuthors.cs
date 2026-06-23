using Catalog.Application.Common;
using Catalog.Application.DTOs.Auhtors;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Authors.Queries
{
    public record GetAuthorsCommand() : IRequest<Result<List<AuthorDto>>>;
    public class GetAuthorsCommandHandler(IAuthorRepository authorRepository) : IRequestHandler<GetAuthorsCommand, Result<List<AuthorDto>>>
    {
        private readonly IAuthorRepository _repository = authorRepository;
        public async Task<Result<List<AuthorDto>>> Handle(GetAuthorsCommand request, CancellationToken cancellationToken)
        {
            var authors = await _repository.GetAllAsync();
            if (authors is null)
                return new Result<List<AuthorDto>>
                {
                    ResultStatus = ResultStatus.NotFound,
                    Data = null,
                    Message = $"No authors found"
                };
            var list = new List<AuthorDto>();
            foreach (var author in authors)
            {
                list.Add(new AuthorDto() { FirstName = author.FirstName, Id = author.Id, LastName = author.LastName, Bio = author.Bio });
            }
            return new Result<List<AuthorDto>>
            {
                ResultStatus = ResultStatus.Success,
                Data = list,
                Message = string.Empty
            };
        }
    }
}
