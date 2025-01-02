using Application.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetUserPermissions;

public class GetUserPermissionQueryHandler(IAppDbContext dbContext)
    : IRequestHandler<GetUserPermissionsQuery, HashSet<PermissionEnum>>
{
    private readonly IAppDbContext _dbContext = dbContext;
    
    public async Task<HashSet<PermissionEnum>> Handle(
        GetUserPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await _dbContext.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == request.UserId)
            .Select(u => u.Roles)
            .ToArrayAsync(cancellationToken);

        return roles
            .SelectMany(r => r)
            .SelectMany(r => r.Permissions)
            .Select(p => (PermissionEnum)p.Id)
            .ToHashSet();
    }
}