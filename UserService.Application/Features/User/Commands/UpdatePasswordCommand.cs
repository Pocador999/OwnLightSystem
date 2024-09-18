using System.Text.Json.Serialization;
using MediatR;
using UserService.Application.Common.Messages;

namespace UserService.Application.Features.User.Commands;

public class UpdatePasswordCommand : IRequest<Messages>
{
    [JsonIgnore]
    public Guid Id { get; set; }

    [JsonIgnore]
    public DateTime UpdatedAt { get; set; }
    public string NewPassword { get; set; }
    public string CurrentPassword { get; set; }
}
