using Basket.Application.Baskets.Commands;
using FluentValidation;

namespace Basket.Application.Validators;

public class UpsertBasketCommandValidator : AbstractValidator<UpsertBasketCommand>
{
    public UpsertBasketCommandValidator()
    {
        RuleFor(x => x.Basket.UserId)
            .NotEmpty();

        RuleFor(x => x.Basket.Items)
            .NotEmpty()
            .WithMessage("Basket must contain at least one item.");

        RuleForEach(x => x.Basket.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.BookId).NotEmpty();
            item.RuleFor(x => x.Title).NotEmpty();
            item.RuleFor(x => x.UnitPrice).GreaterThan(0);
            item.RuleFor(x => x.Quantity).GreaterThan(0);
        });
    }
}