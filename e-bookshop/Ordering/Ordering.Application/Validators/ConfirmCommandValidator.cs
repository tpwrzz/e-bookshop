using FluentValidation;
using Ordering.Application.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Validators
{
    public class ConfirmOrderCommandValidator : AbstractValidator<ConfirmOrderCommand>
    {
        public ConfirmOrderCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
