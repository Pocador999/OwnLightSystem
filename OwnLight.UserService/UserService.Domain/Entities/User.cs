using System.ComponentModel.DataAnnotations;
using UserService.Domain.Primitives;

namespace UserService.Domain.Entities;

public class User : Entity
{
    [Range(3, 50)]
    public string? Name { get; protected set; }

    [Range(3, 50)]
    public string? Username { get; protected set; }

    [Required]
    public string? Email { get; protected set; }

    [Required]
    public string? Password { get; protected set; }


    public ICollection<RefreshToken> Tokens { get; set; } = [];

    public void UpdatePassword(string password)
    {
        Password = password;
        UpdatedAt = DateTime.UtcNow;
    }
}
