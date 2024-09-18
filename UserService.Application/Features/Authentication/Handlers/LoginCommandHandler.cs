using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Common.Messages;
using UserService.Application.Features.Authentication.Command;
using UserService.Domain.Interfaces;
using Entity = UserService.Domain.Entities;

namespace UserService.Application.Features.Authentication.Handlers;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IAuthRepository authRepository,
    IPasswordHasher<Entity.User> passwordHasher
) : IRequestHandler<LoginCommand, Messages>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IPasswordHasher<Entity.User> _passwordHasher = passwordHasher;

    public async Task<Messages> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
        {
            return Messages.Error(
                "Validation Error",
                "Username or password cannot be empty",
                "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                StatusCodes.Status400BadRequest.ToString()
            );
        }

        var user = await _userRepository.FindByUsernameAsync(request.Username);
        if (user == null)
        {
            return Messages.Error(
                "Not Found",
                "User not found",
                "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                StatusCodes.Status404NotFound.ToString()
            );
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.Password,
            request.Password
        );
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            return Messages.Error(
                "Unauthorized",
                "Invalid password",
                "https://tools.ietf.org/html/rfc7235#section-3.1",
                StatusCodes.Status401Unauthorized.ToString()
            );
        }

        await _authRepository.LoginAsync(user.Username, request.Password);

        return Messages.Success(
            "Success",
            "Login successful",
            "https://tools.ietf.org/html/rfc7231#section-6.3.1",
            StatusCodes.Status200OK.ToString()
        );
    }
}
