using System.Text.Json.Serialization;
using MediatR;
using UserService.Application.Common.Services.Messages;

namespace UserService.Application.Features.Authentication.Command;

public class RefreshTokenCommand(string refreshToken) : IRequest<Message>
{
    [JsonIgnore]
    public string RefreshToken { get; set; } = refreshToken;
}
