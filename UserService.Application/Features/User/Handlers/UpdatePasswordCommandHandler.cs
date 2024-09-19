using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Common.Messages;
using UserService.Application.Common.Services.Auth;
using UserService.Application.Common.Validation;
using UserService.Application.Features.User.Commands;
using UserService.Domain.Interfaces;
using Entity = UserService.Domain.Entities;

namespace UserService.Application.Features.User.Handlers;

public class UpdatePasswordCommandHandler(
    IUserRepository userRepository,
    IAuthRepository authRepository,
    IValidator<UpdatePasswordCommand> validator
) : IRequestHandler<UpdatePasswordCommand, Messages>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IValidator<UpdatePasswordCommand> validator = validator;

    public async Task<Messages> Handle(
        UpdatePasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Messages.Error(
                "Validation error",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)),
                "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                "400"
            );
        }

        var user = await _userRepository.FindByIdAsync(request.Id);
        if (user == null)
        {
            return Messages.NotFound(
                "not found",
                "User not found",
                "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                "404"
            );
        }

        var authResult = AuthServices.Authenticate(user);
        if (authResult.StatusCode != StatusCodes.Status200OK.ToString())
            return authResult;

        if (request.CurrentPassword == request.NewPassword)
        {
            return Messages.Error(
                "Validation error",
                "New password must be different from the current password",
                "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                "400"
            );
        }

        var passwordHasher = new PasswordHasher<Entity.User>();
        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(
            user,
            user.Password,
            request.CurrentPassword
        );

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            return Messages.Error(
                "validation error",
                "Current password is incorrect",
                "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                "400"
            );
        }

        request.NewPassword = passwordHasher.HashPassword(user, request.NewPassword);
        await _userRepository.UpdatePasswordAsync(user.Id, request.NewPassword);
        await _authRepository.LogoutAsync(user.Id);

        return Messages.Success(
            "success",
            "Password updated successfully",
            "https://tools.ietf.org/html/rfc7231#section-6.3.1",
            "200"
        );
    }
}
