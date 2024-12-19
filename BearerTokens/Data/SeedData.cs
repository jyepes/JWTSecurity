using BearerTokens.Models;
using Microsoft.AspNetCore.Identity;

namespace BearerTokens.Data;

public static class SeedData
{
    public static async Task SeedPermissionsAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var permissions = new List<Permission>
        {
            new Permission { Name = "Customers.View", Description = "View client information" },
            new Permission { Name = "Customers.Create", Description = "Create new clients" },
            new Permission { Name = "Customers.Update", Description = "Update client information" },
            new Permission { Name = "Customers.Delete", Description = "Delete clients" }
        };

        foreach (var permission in permissions)
        {
            if (!context.Permissions.Any(p => p.Name == permission.Name))
                context.Permissions.Add(permission);
        }

        await context.SaveChangesAsync();
    }
    
    public static async Task AssignPermissionsToRoleAsync(IServiceProvider services, string roleName, List<string> permissionNames)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null) throw new Exception("Role not found");

        foreach (var permissionName in permissionNames)
        {
            var permission = context.Permissions.FirstOrDefault(p => p.Name == permissionName);
            if (permission != null && !context.RolePermissions.Any(rp => rp.RoleId == role.Id && rp.PermissionId == permission.Id))
            {
                context.RolePermissions.Add(new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = permission.Id
                });
            }
        }

        await context.SaveChangesAsync();
    }

}
