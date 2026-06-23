using e_bookshop.Catalog.Application.Common;
using e_bookshop.Catalog.Application.DTOs.Books;
using e_bookshop.Catalog.Domain;
using e_bookshop.Catalog.Domain.Repositories;
using e_bookshop.Domain.Enums;
using MediatR;

namespace e_bookshop.Catalog.Application.Books.Commands
{
    public record UpdateBookPriceCommand(UpdatePriceBookDto UpdateBook) : IRequest<Result>;
    public class UpdateBookPriceCommandHandler(IBookRepository repository) : IRequestHandler<UpdateBookPriceCommand, Result>
    {
        private readonly IBookRepository _repository = repository;

        public async Task<Result> Handle(UpdateBookPriceCommand request, CancellationToken cancellationToken)
        {
            var bookToUpdate = await _repository.GetByIdAsync(request.UpdateBook.Id);
            if (bookToUpdate is null)
                return new Result()
                {
                    ResultStatus = ResultStatus.NotFound,
                    Message = $"Book with Id {request.UpdateBook.Id} was not found."
                };

            Enum.TryParse(request.UpdateBook.NewCurrency, true, out Currencies currency);
            var newPrice = new Money(request.UpdateBook.NewPrice, currency);
            bookToUpdate.UpdatePrice(newPrice);
            await _repository.UpdateAsync(bookToUpdate);
            return new Result()
            {
                ResultStatus = ResultStatus.Created,
                Message = $"Book's Price with Id: {bookToUpdate.Id} is updated to: {bookToUpdate.Price.Currency} {bookToUpdate.Price.Amount}"
            };
        }
    }
}
