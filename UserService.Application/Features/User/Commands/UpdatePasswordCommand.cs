using System.Text.Json.Serialization;
using MediatR;

namespace UserService.Application.Features.User.Commands;

public class UpdatePasswordCommand : IRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Password { get; set; }
}
