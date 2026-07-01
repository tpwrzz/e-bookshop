using FluentValidation;
using Ordering.Application.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Validators
{
    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
