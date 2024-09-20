using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
using UserService.Application.Common.Services.Auth;
using UserService.Application.Common.Services.Messages;
using UserService.Application.Features.User.Commands;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.User.Handlers;

public class UpdateCommandHandler(
    IUserRepository userRepository,
    IAuthRepository authRepository,
    IValidator<UpdateCommand> validator,
    IMessageService messageService,
    AuthServices authServices,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<UpdateCommand, Message>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IValidator<UpdateCommand> _validator = validator;
    private readonly IMapper _mapper = mapper;
    private readonly IMessageService _messageService = messageService;
    private readonly AuthServices _authServices = authServices;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Message> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.Id);

        if (user == null)
            return _messageService.CreateNotFoundMessage("user not found");

        var authResult = _authServices.Authenticate(user);
        if (authResult.StatusCode != StatusCodes.Status200OK.ToString())
            return authResult;

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return _messageService.CreateValidationMessage(
                validationResult.Errors.Select(e => e.ErrorMessage)
            );

        var existingUsername = await _userRepository.FindByUsernameAsync(request.Username);
        if (existingUsername != null && request.Username != user.Username)
            return _messageService.CreateConflictMessage($"{request.Username} already exists");

        var existingEmail = await _userRepository.FindByEmailAsync(request.Email);
        if (existingEmail != null && request.Email != user.Email)
            return _messageService.CreateConflictMessage("Email already exists");

        user.UpdatedAt = DateTime.UtcNow;
        _mapper.Map(request, user);

        await _userRepository.UpdateAsync(user);
        await _authRepository.LogoutAsync(user.Id);
        _httpContextAccessor.HttpContext.Session.Clear();

        return _messageService.CreateSuccessMessage("user updated successfully");
    }
}
