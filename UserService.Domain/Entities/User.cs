using System.ComponentModel.DataAnnotations;
using UserService.Domain.Primitives;

namespace UserService.Domain.Entities;

public class User : Entity
{
    [Range(3, 50)]
    public string Name { get; private set; }

    [Range(3, 50)]
    public string UserName { get; private set; }

    [Required]
    public string Password { get; private set; }
}
