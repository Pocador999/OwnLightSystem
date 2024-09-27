using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using UserService.Application.Common.Services.Auth;
using UserService.Application.Common.Services.Messages;
using UserService.Application.Features.User.Commands.Update;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.User.Handlers.Commands.Update;

public class UpdateEmailCommandHandler(
    IUserRepository userRepository,
    IAuthRepository authRepository,
    IValidator<UpdateEmailCommand> validator,
    IMessageService messageService,
    AuthServices authServices,
    IMapper mapper
) : IRequestHandler<UpdateEmailCommand, Message>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IValidator<UpdateEmailCommand> _validator = validator;
    private readonly IMessageService _messageService = messageService;
    private readonly AuthServices _authServices = authServices;
    private readonly IMapper _mapper = mapper;

    public async Task<Message> Handle(
        UpdateEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.FindByIdAsync(request.Id);

        if (user == null)
            return _messageService.CreateNotFoundMessage("Usuário não encontrado");

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return _messageService.CreateValidationMessage(
                validationResult.Errors.Select(e => e.ErrorMessage)
            );

        var existingEmail = await _userRepository.FindByEmailAsync(request.Email);
        if (existingEmail != null && request.Email == user.Email)
            return _messageService.CreateConflictMessage($"{request.Email} já existe");

        user.UpdatedAt = DateTime.UtcNow;
        _mapper.Map(request, user);

        await _userRepository.UpdateAsync(user);
        await _authRepository.LogoutAsync(user.Id);
        await _authServices.LogoutUserAsync(user.Id);

        return _messageService.CreateSuccessMessage("Email atualizado com sucesso");
    }
}
