using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure;

public class PermissionRequirement(PermissionEnum[] permissions)
    : IAuthorizationRequirement
{
    public PermissionEnum[] Permissions { get; set; } = permissions;
}