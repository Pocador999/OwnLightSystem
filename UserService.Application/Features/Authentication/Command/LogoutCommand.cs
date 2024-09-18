using System.Text.Json.Serialization;
using MediatR;
using UserService.Application.Common.Messages;

namespace UserService.Application.Features.Authentication.Command;

public class LogoutCommand(Guid id) : IRequest<Messages>
{
    [JsonIgnore]
    public Guid Id { get; set; } = id;
}
