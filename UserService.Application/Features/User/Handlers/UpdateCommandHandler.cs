using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using UserService.Application.Common.Messages;
using UserService.Application.Common.Services.Auth;
using UserService.Application.Features.User.Commands;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.User.Handlers;

public class UpdateCommandHandler(
    IUserRepository userRepository,
    IAuthRepository authRepository,
    IValidator<UpdateCommand> validator,
    IMapper mapper
) : IRequestHandler<UpdateCommand, Messages>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IValidator<UpdateCommand> _validator = validator;
    private readonly IMapper _mapper = mapper;

    public async Task<Messages> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.Id);

        if (user == null)
        {
            return Messages.NotFound(
                "Not Found",
                "User not found",
                "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                StatusCodes.Status404NotFound.ToString()
            );
        }

        var authResult = AuthServices.Authenticate(user);
        if (authResult.StatusCode != StatusCodes.Status200OK.ToString())
            return authResult;

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Messages.Error(
                "Validation Error",
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)),
                "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                StatusCodes.Status400BadRequest.ToString()
            );
        }

        var existingUsername = await _userRepository.FindByUsernameAsync(request.Username);
        if (existingUsername != null && request.Username != user.Username)
        {
            return Messages.Error(
                "Conflict",
                $"{request.Username} already exists",
                "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                StatusCodes.Status409Conflict.ToString()
            );
        }
        var existingEmail = await _userRepository.FindByEmailAsync(request.Email);
        if (existingEmail != null && request.Email != user.Email)
        {
            return Messages.Error(
                "Conflict",
                "Email already exists",
                "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                StatusCodes.Status409Conflict.ToString()
            );
        }

        user.UpdatedAt = DateTime.UtcNow;
        _mapper.Map(request, user);
        await _userRepository.UpdateAsync(user);
        await _authRepository.LogoutAsync(user.Id);

        return Messages.Success(
            "Sucess",
            $"{user.Username} updated successfully",
            "https://tools.ietf.org/html/rfc7231#section-6.3.1",
            StatusCodes.Status200OK.ToString()
        );
    }
}
