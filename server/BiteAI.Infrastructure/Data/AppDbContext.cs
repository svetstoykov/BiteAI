using BiteAI.Infrastructure.Models;
using BiteAI.Services.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BiteAI.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<IdentityAccount>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<MealPlan> MealPlans { get; set; }
    public DbSet<MealDay> MealDays { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<IdentityAccount>()
            .HasOne(ia => ia.ApplicationUser)
            .WithOne()
            .HasForeignKey<IdentityAccount>(ia => ia.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<IdentityAccount>()
            .Navigation(ia => ia.ApplicationUser)
            .AutoInclude();

        builder.Entity<IdentityAccount>()
            .Property(p => p.Id)
            .ValueGeneratedNever();
        
        builder.Entity<ApplicationUser>()
            .HasMany(u => u.MealPlans)
            .WithOne()
            .HasForeignKey(mp => mp.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.Entity<MealPlan>()
            .HasMany(mp => mp.MealDays)
            .WithOne(md => md.MealPlan)
            .HasForeignKey(md => md.MealPlanId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<MealDay>()
            .HasMany(md => md.Meals)
            .WithOne(m => m.MealDay)
            .HasForeignKey(m => m.MealDayId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
                
        builder.Entity<ApplicationUser>()
            .Property(u => u.FirstName)
            .IsRequired();
                
        builder.Entity<ApplicationUser>()
            .Property(u => u.LastName)
            .IsRequired();
            
        builder.Entity<ApplicationUser>()
            .Property(u => u.Email)
            .IsRequired();
            
        builder.Entity<ApplicationUser>()
            .Property(u => u.Username)
            .IsRequired();
                
        builder.Entity<MealPlan>()
            .Property(mp => mp.Name)
            .IsRequired();
                
        builder.Entity<Meal>()
            .Property(m => m.Name)
            .IsRequired();
                
        builder.Entity<Meal>()
            .Property(m => m.Description)
            .IsRequired();
                
        builder.Entity<Meal>()
            .Property(m => m.Recipe)
            .IsRequired();
    }
}