using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Role> Roles { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}