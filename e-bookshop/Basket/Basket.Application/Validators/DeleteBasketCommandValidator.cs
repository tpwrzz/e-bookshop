using Basket.Application.Baskets.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Validators
{
    public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
