using FluentValidation;
using UserService.Application.Common.Email;
using UserService.Application.Features.User.Commands;

namespace UserService.Application.Common.Validation;

public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .MaximumLength(30)
            .NotEmpty()
            .WithMessage("Name must be between 3 and 30 characters");

        RuleFor(x => x.Username)
            .MinimumLength(3)
            .MaximumLength(30)
            .NotEmpty()
            .WithMessage("Username must be between 3 and 30 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email must not be empty")
            .EmailAddress()
            .WithMessage("Email must be a valid email address")
            .Must(x => !x.Contains(' '))
            .WithMessage("Email must not contain spaces");

        RuleFor(x => x.Email)
            .Must(EmailVerifier.IsValidDomain)
            .WithMessage("Email domain is invalid");
    }
}
