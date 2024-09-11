namespace UserService.Application.DTOs;

public class UserResponseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
