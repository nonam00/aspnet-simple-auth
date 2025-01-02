using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityTypeConfigurations;

public class RolePermissionConfiguration(AuthorizationOptions authorization)
    : IEntityTypeConfiguration<RolePermissionEntity>
{
    private readonly AuthorizationOptions _authorizationOptions = authorization;
    public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
    {
        builder.HasKey(r => new { r.RoleId, r.PermissionId });

        builder.HasData(ParseRolePermissions());
    }

    private List<RolePermissionEntity> ParseRolePermissions()
    {
        return _authorizationOptions.RolePermissions
            .SelectMany(rp => rp.Permission
                .Select(p => new RolePermissionEntity
                {
                    RoleId = (int)Enum.Parse<RoleEnum>(rp.Role),
                    PermissionId = (int)Enum.Parse<PermissionEnum>(p)
                }))
            .ToList();
    }
}