using FluentValidation;
using Ordering.Application.Orders.Commands;

namespace Ordering.Application.Validators;

public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.Order.UserId)
            .NotEmpty();

        RuleFor(x => x.Order.Address.Street)
            .NotEmpty().MaximumLength(200);

        RuleFor(x => x.Order.Address.City)
            .NotEmpty().MaximumLength(100);

        RuleFor(x => x.Order.Address.Country)
            .NotEmpty().MaximumLength(100);

        RuleFor(x => x.Order.Address.Postcode)
            .NotEmpty().MaximumLength(20);

        RuleFor(x => x.Order.OrderItems)
            .NotEmpty()
            .WithMessage("Order must contain at least one item.");

        RuleForEach(x => x.Order.OrderItems).ChildRules(item =>
        {
            item.RuleFor(x => x.Title).NotEmpty();
            item.RuleFor(x => x.Price).GreaterThan(0);
            item.RuleFor(x => x.Quantity).GreaterThan(0);
        });
    }
}