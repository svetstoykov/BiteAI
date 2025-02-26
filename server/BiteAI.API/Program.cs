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
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityAccount>>();
        
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
async Task SeedDefaultAdmin(UserManager<IdentityAccount> userManager, IConfiguration configuration)
{
    // Check if admin user exists
    var adminEmail = configuration["DefaultAdmin:Email"] ?? 
        throw new InvalidOperationException("DefaultAdmin:Email not configured");
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    
    if (adminUser == null)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // Start a transaction to ensure both entities are created
        using var transaction = await dbContext.Database.BeginTransactionAsync();
        
        try
        {
            var userId = Guid.NewGuid().ToString();
            
            // Create ApplicationUser
            var applicationUser = new BiteAI.Services.Entities.ApplicationUser
            {
                Id = userId,
                FirstName = configuration["DefaultAdmin:FirstName"] ?? "Admin",
                LastName = configuration["DefaultAdmin:LastName"] ?? "User",
                Email = adminEmail,
                Username = configuration["DefaultAdmin:UserName"] ?? "admin",
                CreatedAt = DateTime.UtcNow,
                Gender = BiteAI.Services.Enums.Gender.Male,  // Default value
                ActivityLevel = BiteAI.Services.Enums.ActivityLevel.NotSpecified
            };
            
            // Add ApplicationUser to database
            await dbContext.ApplicationUsers.AddAsync(applicationUser);
            await dbContext.SaveChangesAsync();
            
            // Create IdentityAccount
            var identityAccount = new IdentityAccount
            {
                Id = userId,
                UserName = configuration["DefaultAdmin:UserName"] ?? "admin",
                Email = adminEmail,
                EmailConfirmed = true,
                ApplicationUserId = userId
            };
            
            // Create the identity account with password
            var adminPassword = configuration["DefaultAdmin:Password"] ?? 
                throw new InvalidOperationException("DefaultAdmin:Password not configured");
            var result = await userManager.CreateAsync(identityAccount, adminPassword);
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(identityAccount, "Admin");
                await transaction.CommitAsync();
            }
            else
            {
                await transaction.RollbackAsync();
                // Log the error or handle it appropriately
                Console.WriteLine($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            // Log the error or handle it appropriately
            Console.WriteLine($"Error seeding admin user: {ex.Message}");
        }
    }
}