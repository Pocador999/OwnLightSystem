using MediatR;
using UserService.Application.Features.Admin.Commands;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.Admin.Handlers;

public class DeleteAllCommandHandler(IUserRepository userRepository)
    : IRequestHandler<DeleteAllCommand>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Unit> Handle(DeleteAllCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteAllAsync();
        return Unit.Value;
    }
}
