using BiteAI.Infrastructure.Models;
using BiteAI.Services.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BiteAI.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSets for domain entities
    public DbSet<MealPlan> MealPlans { get; set; }
    public DbSet<MealDay> MealDays { get; set; }
    public DbSet<Meal> Meals { get; set; }
        
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
            
        // Configure MealPlan -> MealDay relationship
        builder.Entity<MealPlan>()
            .HasMany(mp => mp.MealDays)
            .WithOne(md => md.MealPlan)
            .HasForeignKey(md => md.MealPlanId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
                
        builder.Entity<MealPlan>()
            .HasOne<ApplicationUser>()
            .WithMany(u => u.MealPlans)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder.Entity<MealDay>()
            .HasMany(md => md.Meals)
            .WithOne(m => m.MealDay)
            .HasForeignKey(m => m.MealDayId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
                
        // Configure entities with required fields
        builder.Entity<ApplicationUser>()
            .Property(u => u.FirstName)
            .IsRequired();
                
        builder.Entity<ApplicationUser>()
            .Property(u => u.LastName)
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