using Bookshop.SharedKernel.Domain.Enums;
using Catalog.Application.Books.Commands;
using FluentValidation;

namespace Catalog.Application.Validators.Books
{
    public class UpdateBookPriceCommandValidator : AbstractValidator<UpdateBookPriceCommand>
    {
        public UpdateBookPriceCommandValidator()
        {
            RuleFor(x => x.UpdateBook.NewPrice)
                    .GreaterThan(0);

            RuleFor(x => x.UpdateBook.NewCurrency)
                    .NotEmpty()
                    .Must(c => Enum.TryParse<Currency>(c, true, out _))
                    .WithMessage("'{PropertyValue}' is not a valid currency code.");
        }
    }
}