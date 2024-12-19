namespace BearerTokens.Models;

public class Permission
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty; // e.g., "Customers.View"
    public string Description { get; set; } = string.Empty; // Optional
}