using BiteAI.API.Extensions;
using BiteAI.API.Middleware;
using BiteAI.Infrastructure.Data;
using BiteAI.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services
    .AddOpenApi()
    .AddApplicationServices()
    .AddFirebaseServices(builder.Configuration)
    .AddAnthropicService(builder.Configuration)
    .AddDatabaseServices(builder.Configuration); // Add database and identity services

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var corsOrigins = builder.Configuration.GetSection("CorsOrigins").Get<Dictionary<string, string[]>>()!;

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy", policy =>
    {
        policy.WithOrigins(corsOrigins["Development"])
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors("DevelopmentPolicy");
    
    // Automatically apply migrations in development
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
        
    // Seed default roles
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
    await SeedDefaultRoles(roleManager);
    await SeedDefaultAdmin(userManager, builder.Configuration);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

// Seed default roles method
async Task SeedDefaultRoles(RoleManager<IdentityRole> roleManager)
{
    string[] roleNames = { "Admin", "User" };
    
    foreach (var roleName in roleNames)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// Seed default admin method
async Task SeedDefaultAdmin(UserManager<ApplicationUser> userManager, IConfiguration configuration)
{
    // Check if admin user exists
    var adminEmail = configuration["DefaultAdmin:Email"] ?? 
        throw new InvalidOperationException("DefaultAdmin:Email not configured");
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    
    if (adminUser == null)
    {
        var admin = new ApplicationUser
        {
            UserName = configuration["DefaultAdmin:UserName"] ?? "admin",
            Email = adminEmail,
            FirstName = configuration["DefaultAdmin:FirstName"] ?? "Admin",
            LastName = configuration["DefaultAdmin:LastName"] ?? "User",
            EmailConfirmed = true
        };
        
        var adminPassword = configuration["DefaultAdmin:Password"] ?? 
            throw new InvalidOperationException("DefaultAdmin:Password not configured");
        var result = await userManager.CreateAsync(admin, adminPassword);
        
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}