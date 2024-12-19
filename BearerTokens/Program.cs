using BearerTokens.Data;
using BearerTokens.Extensions;
using BearerTokens.Middlewares;
using BearerTokens.Models;
using Carter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Entity Framework Core configuration
builder.Services.AddDbContext<AppDbContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Authentication and Authorization configuration
builder.Services
    .AddAuthorization()
    .AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);

// Identity Core configuration
builder.Services
    .AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddCarter();

var app = builder.Build();

// Initialize seed data
await app.Services.SeedDatabaseAsync();
// Configure the HTTP request pipeline.

// Map OpenAPI and Scalar API reference in Development environment
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Portal Api")
            .WithTheme(ScalarTheme.Default)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithDarkMode(false)
            .WithEndpointPrefix("/api/{documentName}");
    });
}

// Ensure HTTPS
app.UseHttpsRedirection();

// Configure authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Map Authentication endpoints
app.MapIdentityApi<User>();

// Map application-specific routes
app.MapCarter();

app.Run();


