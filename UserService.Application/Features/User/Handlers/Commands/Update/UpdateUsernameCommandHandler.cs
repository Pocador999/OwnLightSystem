using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using UserService.Application.Common.Services.Messages;
using UserService.Application.Features.User.Commands.Update;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.User.Handlers.Commands.Update;

public class UpdateUsernameCommandHandler(
    IUserRepository userRepository,
    IAuthRepository authRepository,
    IMessageService messageService,
    IValidator<UpdateUsernameCommand> validator,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper
) : IRequestHandler<UpdateUsernameCommand, Message>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IMessageService _messageService = messageService;
    private readonly IValidator<UpdateUsernameCommand> _validator = validator;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IMapper _mapper = mapper;

    public async Task<Message> Handle(
        UpdateUsernameCommand request,
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

        var existingUsername = await _userRepository.FindByUsernameAsync(request.Username);
        if (existingUsername != null && request.Username == user.Username)
            return _messageService.CreateConflictMessage($"{request.Username} já existe");

        user.UpdatedAt = DateTime.UtcNow;
        _mapper.Map(request, user);

        await _userRepository.UpdateAsync(user);
        await _authRepository.LogoutAsync(user.Id);
        _httpContextAccessor.HttpContext.Session.Clear();

        return _messageService.CreateSuccessMessage("Nome de usuário atualizado com sucesso");
    }
}
