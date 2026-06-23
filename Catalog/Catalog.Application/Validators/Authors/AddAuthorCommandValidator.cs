using Catalog.Application.Authors.Commands;
using FluentValidation;

namespace Catalog.Application.Validators.Authors
{
    public class AddAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
    {
        public AddAuthorCommandValidator()
        {
            RuleFor(x => x.Author.FirstName)
                    .NotEmpty()
                    .MaximumLength(40);
            RuleFor(x => x.Author.LastName)
                 .NotEmpty()
                 .MaximumLength(40);
            RuleFor(x => x.Author.Bio)
                 .MaximumLength(600);
        }
    }
}
