using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Features.User.Queries;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.User.Handlers;

public class GetByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<GetByIdQuery, UserResponseDTO>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<UserResponseDTO> Handle(
        GetByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var user =
            await _userRepository.FindByIdAsync(request.Id)
            ?? throw new DllNotFoundException("User not found");
        return _mapper.Map<UserResponseDTO>(user);
    }
}
