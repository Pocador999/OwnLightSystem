using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using UserService.Domain.Primitives;

namespace UserService.Domain.Entities;

public class User : Entity
{
    [Range(3, 50)]
    public string Name { get; protected set; }

    [Range(3, 50)]
    public string Username { get; protected set; }

    [Required]
    public string Email { get; protected set; }

    [Required]
    public string Password { get; protected set; }

    public bool IsLogedIn { get; protected set; } = false;
    public DateTime LastLoginAt { get; protected set; }

    public void UpdatePassword(string password)
    {
        Password = password;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Login()
    {
        IsLogedIn = true;
        LastLoginAt = DateTime.UtcNow;
    }

    public void Logout() => IsLogedIn = false;
}
