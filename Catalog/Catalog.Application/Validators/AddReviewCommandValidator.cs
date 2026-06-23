using FluentValidation;
using static e_bookshop.Catalog.Application.Books.Commands.AddReview;

namespace e_bookshop.Catalog.Application.Validators
{
    public class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
    {
        public AddReviewCommandValidator()
        {
            RuleFor(x => x.Review.Message)
               .NotEmpty()
               .MaximumLength(1000);

            RuleFor(x => x.Review.Rating).GreaterThan(0)
                .LessThanOrEqualTo(5)
                .WithMessage("Rating should be between 1 and 5.");
        }
    }
}
