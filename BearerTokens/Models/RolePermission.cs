using Microsoft.AspNetCore.Identity;

namespace BearerTokens.Models;

public class RolePermission
{
    public string RoleId { get; set; } // Foreign Key to IdentityRole
    public IdentityRole Role { get; set; }
    
    public Guid PermissionId { get; set; } // Foreign Key to Permission
    public Permission Permission { get; set; }
}