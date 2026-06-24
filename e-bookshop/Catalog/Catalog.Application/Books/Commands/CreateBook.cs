using Bookshop.SharedKernel.Application.Common;
using Bookshop.SharedKernel.Domain;
using Catalog.Application.DTOs.Books;
using Catalog.Domain;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Books.Commands
{
    public record CreateBookCommand(CreateBookDto CreateBook) : IRequest<Result>;

    public class CreateBookCommandHandler(IBookRepository repository, IAuthorRepository authorRepository) : IRequestHandler<CreateBookCommand, Result>
    {
        private readonly IBookRepository _repository = repository;
        private readonly IAuthorRepository _authorRepository = authorRepository;

        public async Task<Result> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(request.CreateBook.AuthorId);
            if (author is null)
                return new Result()
                {
                    ResultStatus = ResultStatus.NotFound,
                    Message = $"Author with Id {request.CreateBook.AuthorId} was not found."
                };
            var book = new Book(id: Guid.NewGuid(),
                                title: request.CreateBook.Title,
                                description: request.CreateBook.Description,
                                genre: request.CreateBook.Genre,
                                pageCount: request.CreateBook.PageCount,
                                price: new Money(request.CreateBook.Price, request.CreateBook.Currency),
                                language: request.CreateBook.Language,
                                availability: true,
                                publicationDate: request.CreateBook.PublicationDate,
                                author: author);

            await _repository.AddAsync(book);

            return new Result()
            {
                ResultStatus = ResultStatus.Created,
                Message = $"New Book was created with Id: {book.Id}"
            };
        }
    }
}
