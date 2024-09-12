using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Features.User.Commands;
using UserService.Domain.Interfaces;
using Entity = UserService.Domain.Entities;

namespace UserService.Application.Features.User.Handlers;

public class UpdatePasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher<Entity.User> passwordHasher
) : IRequestHandler<UpdatePasswordCommand>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher<Entity.User> _passwordHasher = passwordHasher;

    public async Task<Unit> Handle(
        UpdatePasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var user =
            await _userRepository.FindByIdAsync(request.Id)
            ?? throw new Exception("User not found");
        var passwordHash = _passwordHasher.HashPassword(user, request.Password);
        await _userRepository.UpdatePasswordAsync(user.Id, passwordHash);
        return Unit.Value;
    }
}
