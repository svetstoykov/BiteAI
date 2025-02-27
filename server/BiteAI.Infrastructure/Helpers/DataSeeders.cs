using BiteAI.Infrastructure.Models;
using BiteAI.Services.Constants;
using BiteAI.Services.Contracts.Authentication;
using BiteAI.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BiteAI.Infrastructure.Helpers;

public class DataSeeders
{
    public static async Task AddDefaultRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = [Roles.Admin, Roles.User];

        foreach (var roleName in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    public static async Task AddDefaultAdminAsync(WebApplication app, UserManager<IdentityAccount> userManager, IConfiguration configuration)
    {
        var adminEmail = configuration["DefaultAdmin:Email"] ?? throw new InvalidOperationException("DefaultAdmin:Email not configured");
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            using var scope = app.Services.CreateScope();
            var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();
            
            var password = configuration["DefaultAdmin:Password"] ?? throw new InvalidOperationException("DefaultAdmin:Password not configured");
            var applicationUser = new RegisterDto
            {
                FirstName = configuration["DefaultAdmin:FirstName"] ?? "Admin",
                LastName = configuration["DefaultAdmin:LastName"] ?? "User",
                Email = adminEmail,
                Username = configuration["DefaultAdmin:UserName"] ?? "admin",
                Password = password,
                ConfirmPassword = password,
                Gender = BiteAI.Services.Enums.Gender.Male, 
                ActivityLevel = BiteAI.Services.Enums.ActivityLevel.NotSpecified
            };

            
            await identityService.RegisterUserAsync(applicationUser);
        }
    }
}