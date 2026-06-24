using Bookshop.SharedKernel.Application.Common;
using Catalog.Application.DTOs.Auhtors;
using Catalog.Application.DTOs.Books;
using Catalog.Application.DTOs.Reviews;
using Catalog.Domain;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Books.Queries
{
    public class GetBooks
    {
        public record GetBooksQuery(BookFilterDto Filter) : IRequest<Result<PagedResult<BookDto>>>;

        public class GetBooksQueryHandler(IBookRepository repository) : IRequestHandler<GetBooksQuery, Result<PagedResult<BookDto>>>
        {
            private readonly IBookRepository _repository = repository;

            public async Task<Result<PagedResult<BookDto>>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
            {
                var (books, totalCount) = await _repository.GetPagedAsync(
                    page: request.Filter.Pagination.Page,
                    pageSize: request.Filter.Pagination.PageSize,
                    genre: request.Filter.Genre,
                    availability: request.Filter.Availability,
                    rating: request.Filter.Rating,
                    language: request.Filter.Language,
                    authorName: request.Filter.AuthorName,
                    cancellationToken: cancellationToken);

                var bookDtos = books.Select(b => MapToDto(b)).ToList();

                var pagedResult = new PagedResult<BookDto>
                {
                    Items = bookDtos,
                    TotalCount = totalCount,
                    Page = request.Filter.Pagination.Page,
                    PageSize = request.Filter.Pagination.PageSize
                };

                return new Result<PagedResult<BookDto>>
                {
                    ResultStatus = ResultStatus.Success,
                    Data = pagedResult,
                    Message = string.Empty
                };
            }
            private BookDto MapToDto(Book book)
            {
                return new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
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
                    },
                    Reviews = book.Reviews.Select(r => new ReviewDto
                    {
                        Id = r.Id,
                        Message = r.Message,
                        Rating = r.Rating.Value,
                        UserId = r.UserId,
                        CreatedAt = r.CreatedAt,
                        UpdatedAt = r.UpdatedAt
                    }).ToList()
                };
            }
        }
    }
}
