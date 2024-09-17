using System;
using MediatR;

namespace UserService.Application.Features.Authentication.Command;

public class LoginCommand : IRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
