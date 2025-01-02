using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence.EntityTypeConfigurations;

namespace Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options,
    IOptions<AuthorizationOptions> authOptions)
    : DbContext(options), IAppDbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(authOptions.Value));
        
        base.OnModelCreating(modelBuilder);
    }
}