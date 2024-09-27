using System.Text.Json.Serialization;
using MediatR;
using UserService.Application.Common.Services.Messages;

namespace UserService.Application.Features.Authentication.Command;

public class RefreshTokenCommand : IRequest<Message>
{
    [JsonIgnore]
    public string RefreshToken { get; set; } = string.Empty;
}
