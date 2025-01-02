using Domain.Enums;
using MediatR;

namespace Application.Users.Queries.GetUserPermissions;

public class GetUserPermissionsQuery : IRequest<HashSet<PermissionEnum>>
{
    public Guid UserId { get; set; }
}