using Bookshop.SharedKernel.Application.Common;
using Bookshop.SharedKernel.Domain;
using Bookshop.SharedKernel.Domain.Enums;
using Catalog.Application.DTOs.Books;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Books.Commands
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

            Enum.TryParse(request.UpdateBook.NewCurrency, true, out Currency currency);
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
