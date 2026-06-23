using FluentValidation;
using static Catalog.Application.Books.Commands.UpdateReview;

namespace Catalog.Application.Validators.Books
{
    public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
    {
        public UpdateReviewCommandValidator()
        {
            RuleFor(x => x.Review.Message)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.Review.Rating)
                .GreaterThan(0)
                .LessThanOrEqualTo(5);

            RuleFor(x => x.Review.UserId)
                .NotEmpty();

            RuleFor(x => x.Review.ReviewId)
                .NotEmpty();
        }
    }
}
