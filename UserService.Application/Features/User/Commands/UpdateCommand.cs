using System.Text.Json.Serialization;
using MediatR;
using UserService.Application.Common.Messages;
using UserService.Application.DTOs;

namespace UserService.Application.Features.User.Commands;

public class UpdateCommand : IRequest<Messages>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
}
