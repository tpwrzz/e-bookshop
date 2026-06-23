using e_bookshop.Catalog.Application.Books.Commands;
using e_bookshop.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace e_bookshop.Catalog.Application.Validators
{
    public class UpdateBookPriceCommandValidator : AbstractValidator<UpdateBookPriceCommand>
    {
        public UpdateBookPriceCommandValidator()
        {
            RuleFor(x => x.UpdateBook.NewPrice)
                    .GreaterThan(0);

            RuleFor(x => x.UpdateBook.NewCurrency)
                    .NotEmpty()
                    .Must(c => Enum.TryParse<Currencies>(c, true, out _))
                    .WithMessage("'{PropertyValue}' is not a valid currency code.");
        }
    }
}