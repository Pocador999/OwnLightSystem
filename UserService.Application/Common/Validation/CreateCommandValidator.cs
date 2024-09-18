using FluentValidation;
using UserService.Application.Features.User.Commands;

namespace UserService.Application.Common.Validation;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name must not be empty")
            .Length(3, 30)
            .WithMessage("Name must be between 3 and 30 characters");

        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username must not be empty")
            .Length(3, 30)
            .WithMessage("Username must be between 3 and 30 characters")
            .Matches("^[a-zA-Z0-9]*$")
            .WithMessage("Username must not contain special characters or spaces");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password must not be empty")
            .Length(6, 20)
            .WithMessage("Password must be between 6 and 20 characters");
    }
}
