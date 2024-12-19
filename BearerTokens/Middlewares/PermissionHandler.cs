using System.Security.Claims;
using BearerTokens.Data;
using Microsoft.AspNetCore.Authorization;

namespace BearerTokens.Middlewares;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceProvider _serviceProvider;

    public PermissionHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        // Crear un nuevo scope para obtener el DbContext
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Lógica para verificar permisos
        var user = context.User;
        if (user == null) return;

        // Recuperar permisos del usuario
        var roles = context.User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        var permissions = dbContext.RolePermissions
            .Where(rp => roles.Contains(rp.Role.Name))
            .Select(rp => rp.Permission.Name)
            .ToList();

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}


public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        Permission = permission;
    }
}
