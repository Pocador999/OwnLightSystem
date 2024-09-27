using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using UserService.Application.Common.Services.Auth;
using UserService.Application.Common.Services.Messages;
using UserService.Application.Features.User.Commands.Update;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.User.Handlers.Commands.Update;

public class UpdateCommandHandler(
    IUserRepository userRepository,
    IAuthRepository authRepository,
    IValidator<UpdateCommand> validator,
    IMessageService messageService,
    AuthServices authServices,
    IMapper mapper
) : IRequestHandler<UpdateCommand, Message>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IValidator<UpdateCommand> _validator = validator;
    private readonly IMapper _mapper = mapper;
    private readonly IMessageService _messageService = messageService;
    private readonly AuthServices _authServices = authServices;

    public async Task<Message> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(request.Id);

        if (user == null)
            return _messageService.CreateNotFoundMessage("Usuário não encontrado");

        var authResult = _authServices.Authenticate(user);
        if (authResult.StatusCode != StatusCodes.Status200OK.ToString())
            return authResult;
            
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return _messageService.CreateValidationMessage(
                validationResult.Errors.Select(e => e.ErrorMessage)
            );

        if (request.Name == user.Name)
            return _messageService.CreateConflictMessage($"{request.Name} já existe");

        user.UpdatedAt = DateTime.UtcNow;
        _mapper.Map(request, user);

        await _userRepository.UpdateAsync(user);
        await _authRepository.LogoutAsync(user.Id);

        return _messageService.CreateSuccessMessage("Usuário atualizado com sucesso");
    }
}
