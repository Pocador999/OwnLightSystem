using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Features.User.Queries;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.User.Handlers;

public class GetByUserNameQueryHandler(IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<GetByUserNameQuery, UserResponseDTO>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<UserResponseDTO> Handle(
        GetByUserNameQuery request,
        CancellationToken cancellationToken
    )
    {
        var user =
            await _userRepository.FindByUserNameAsync(request.UserName)
            ?? throw new Exception("User not found");
        return _mapper.Map<UserResponseDTO>(user);
    }
}
