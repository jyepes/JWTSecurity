using BearerTokens.Data;

namespace BearerTokens.Extensions;

public static class ApplicationSeederExtensions
{
    public static async Task SeedDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var scopedServices = scope.ServiceProvider;

        var logger = scopedServices.GetRequiredService<ILogger<Program>>();
        try
        {
            // Inicializar los datos semilla
            await SeedData.SeedPermissionsAsync(scopedServices);

            // Asignar permisos a roles
            await SeedData.AssignPermissionsToRoleAsync(scopedServices, "Admin", new List<string>
            {
                "Customers.View", "Customers.Create", "Customers.Update", "Customers.Delete"
            });

            logger.LogInformation("Datos semilla inicializados correctamente.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocurrió un error al inicializar los datos semilla.");
        }
    }
}
