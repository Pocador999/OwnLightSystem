using System;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Features.Authentication.Command;
using UserService.Domain.Interfaces;
using UserService.Domain.Primitives;
using Entity = UserService.Domain.Entities;

namespace UserService.Application.Features.Authentication.Handlers;

public class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher<Entity.User> passwordHasher
) : IRequestHandler<LoginCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher<Entity.User> _passwordHasher = passwordHasher;

    public async Task<Unit> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user =
            await _userRepository.FindByUserNameAsync(request.UserName)
            ?? throw new Exception("User not found");

        if (
            _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password)
            == PasswordVerificationResult.Failed
        )
            throw new Exception("Invalid password");

        return Unit.Value;
    }
}
