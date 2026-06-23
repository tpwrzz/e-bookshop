using e_bookshop.Catalog.Application.Books.Commands;
using FluentValidation;

namespace e_bookshop.Catalog.Application.Validators
{
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor(x => x.CreateBook.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.CreateBook.Price)
                .GreaterThan(0);

            RuleFor(x => x.CreateBook.PublicationDate).NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Publication date cannot be in the future.");

            RuleFor(x => x.CreateBook.AuthorId)
                .NotEmpty()
                .WithMessage("AuthorId is required.");

            RuleFor(x => x.CreateBook.PageCount)
                .GreaterThan(0);

        }
    }
}
