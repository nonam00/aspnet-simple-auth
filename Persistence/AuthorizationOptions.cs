namespace Persistence;

public class AuthorizationOptions
{
    public RolePermissions[] RolePermissions { get; set; }
}

public class RolePermissions
{
    public string Role { get; set; } = null!;
    public string[] Permission { get; set; } = [];
}