using Application.Users.Queries.GetUserPermissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    { 
        var userId = context.User.Claims.FirstOrDefault(
            c => c.Type == CustomClaims.UserId);

        if (userId is null || !Guid.TryParse(userId.Value, out var id))
        {
            return;
        }
        
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        var query = new GetUserPermissionsQuery
        {
            UserId = id
        };
        
        var permissions = await mediator.Send(query);

        foreach (var permission in requirement.Permissions)
        {
            if (!permissions.Contains(permission))
            {
                return;
            }
        }
        
        context.Succeed(requirement);
    }
}