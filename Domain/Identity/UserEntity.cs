namespace Domain.Identity;

public class UserEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<RoleEntity> Roles { get; set; } = new List<RoleEntity>();
}
