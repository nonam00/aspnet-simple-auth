using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityTypeConfigurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(role => role.Id);

        builder.HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity<RolePermissionEntity>(
                l => l.HasOne<Permission>().WithMany().HasForeignKey(p => p.PermissionId),
                r => r.HasOne<Role>().WithMany().HasForeignKey(p => p.RoleId));
        
        var roles = Enum
            .GetValues<RoleEnum>()
            .Select(r => new Role
            {
                Id = (int)r,
                Name = r.ToString()
            });
        
        builder.HasData(roles);
    }
}