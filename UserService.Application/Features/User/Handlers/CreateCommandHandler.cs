using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using UserService.Application.Common.Messages;
using UserService.Application.Features.User.Commands;
using UserService.Domain.Interfaces;
using Entity = UserService.Domain.Entities;

namespace UserService.Application.Features.User.Handlers;

public class CreateCommandHandler(
    IMapper mapper,
    IUserRepository userRepository,
    IPasswordHasher<Entity.User> passwordHasher,
    IValidator<CreateCommand> validator
) : IRequestHandler<CreateCommand, Messages>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher<Entity.User> _passwordHasher = passwordHasher;
    private readonly IValidator<CreateCommand> _validator = validator;

    public async Task<Messages> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
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

        request.Password = _passwordHasher.HashPassword(new Entity.User(), request.Password);
        await _userRepository.RegisterAsync(_mapper.Map<Entity.User>(request));

        return Messages.Success(
            "Success",
            "User created successfully",
            "https://tools.ietf.org/html/rfc7231#section-6.3.1",
            StatusCodes.Status201Created.ToString()
        );
    }
}
