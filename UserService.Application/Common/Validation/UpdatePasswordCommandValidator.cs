using FluentValidation;
using UserService.Application.Features.User.Commands;

namespace UserService.Application.Common.Validation;

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword)
            .MinimumLength(6)
            .MaximumLength(30)
            .NotEmpty()
            .WithMessage("Password must be between 6 and 30 characters");
    }
}
