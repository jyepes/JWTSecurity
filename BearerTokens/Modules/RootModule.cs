using Carter;

namespace BearerTokens.Modules;

public class RootModule: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => Results.Redirect("/api/v1"));
        
        app.MapGet("/me", (HttpContext httpContext) =>
            {
                return new
                {
                    httpContext.User.Identity!.Name,
                    httpContext.User.Identity.AuthenticationType,
                    Claims = httpContext.User.Claims.Select(s => new
                    {
                        s.Type, s.Value
                    }).ToList()
                };
            })
            .RequireAuthorization();
    }
}