using System.ComponentModel.DataAnnotations;

namespace Ramboe.IS4.Data.Models;

public class RoleEntity
{
    public Guid Id { get; set; }

    [Required] public string Name { get; set; }

    public ICollection<UserEntity> Users { get; set; }
}
