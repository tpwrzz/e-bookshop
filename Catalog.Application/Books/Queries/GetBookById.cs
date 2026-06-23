using e_bookshop.Catalog.Application.Common;
using e_bookshop.Catalog.Application.DTOs;
using e_bookshop.Catalog.Application.DTOs.Books;
using e_bookshop.Catalog.Application.DTOs.Reviews;
using e_bookshop.Catalog.Domain.Repositories;
using MediatR;

namespace e_bookshop.Catalog.Application.Books.Queries
{
    public record GetBookByIdQuery(Guid Id) : IRequest<Result<BookDto>>;

    public class GetBookByIdQueryHandler(IBookRepository repository) : IRequestHandler<GetBookByIdQuery, Result<BookDto>>
    {
        private readonly IBookRepository _repository = repository;

        async Task<Result<BookDto>> IRequestHandler<GetBookByIdQuery, Result<BookDto>>.Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _repository.GetByIdAsync(request.Id);

            if (book is null)
            {
                return new Result<BookDto>()
                {
                    ResultStatus = ResultStatus.NotFound,
                    Data = null,
                    Message = $"Book with Id: {request.Id} not found"
                };
            }
            var bookDto = new BookDto()
            {
                Id = book.Id,
                Description = book.Description,
                Title = book.Title,
                Language = book.Language,
                Genre = book.Genre,
                PageCount = book.PageCount,
                Price = book.Price.Amount,
                Currency = book.Price.Currency.ToString(),
                AverageRating = book.AverageRating,
                PublicationDate = book.PublicationDate.ToShortDateString(),
                Availability = book.Availability,
                Author = new AuthorDto
                {
                    Id = book.Author.Id,
                    FirstName = book.Author.FirstName,
                    LastName = book.Author.LastName,
                    Bio = book.Author.Bio
                }
            };

            foreach (var review in book.Reviews)
            {
                bookDto.Reviews.Add(new ReviewDto()
                {
                    Id = review.Id,
                    CreatedAt = review.CreatedAt,
                    Message = review.Message,
                    Rating = review.Rating.Value,
                    UpdatedAt = review.UpdatedAt,
                    UserId = review.UserId

                });
            }

            return new Result<BookDto>()
            {
                ResultStatus = ResultStatus.Success,
                Message = string.Empty,
                Data = bookDto
            };
        }
    }
}

