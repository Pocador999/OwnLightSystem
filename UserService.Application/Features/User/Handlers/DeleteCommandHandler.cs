using MediatR;
using UserService.Application.Features.User.Commands;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.User.Handlers;

public class DeleteCommandHandler(IUserRepository userRepository)
    : IRequestHandler<DeleteCommand, Unit>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Unit> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.DeleteAsync(request.Id) 
            ?? throw new Exception("User not found");
        return Unit.Value;
    }
}
