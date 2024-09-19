namespace UserService.Application.DTOs;

public class UserResponseDTO
{
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsLogedIn { get; set; }
}
