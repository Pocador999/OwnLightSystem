using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using UserService.Application.Common.Messages;
using UserService.Application.Features.User.Commands;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.User.Handlers;

public class UpdateCommandHandler(
    IUserRepository userRepository,
    IValidator<UpdateCommand> validator,
    IMapper mapper
) : IRequestHandler<UpdateCommand, Messages>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator<UpdateCommand> _validator = validator;
    private readonly IMapper _mapper = mapper;

    public async Task<Messages> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errorMessage = new Messages(
                "Validation Error",
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)),
                "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                StatusCodes.Status400BadRequest.ToString()
            );
            return errorMessage;
        }

        var user = await _userRepository.FindByIdAsync(request.Id);
        if (user == null)
        {
            var errorMessage = new Messages(
                "Error",
                "User not found",
                "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                StatusCodes.Status404NotFound.ToString()
            );
            return errorMessage;
        }
        user.UpdatedAt = DateTime.UtcNow;
        _mapper.Map(request, user);
        await _userRepository.UpdateAsync(user);

        var successMessage = new Messages(
            "Success",
            "User updated successfully",
            "https://tools.ietf.org/html/rfc7231#section-6.3.1",
            StatusCodes.Status200OK.ToString()
        );

        return successMessage;
    }
}
