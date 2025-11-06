using BiteAI.Services.Contracts.Enums;
using BiteAI.Services.Data.Entities;
using BiteAI.Services.Data.Entities.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BiteAI.Services.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<MealPlan> MealPlans { get; set; }
    public DbSet<MealDay> MealDays { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<ActivityLevelTypeEntity> ActivityLevels { get; set; }
    public DbSet<DietTypeEntity> DietTypes { get; set; }
    public DbSet<GenderTypeEntity> Genders { get; set; }
    public DbSet<MealTypeEntity> MealTypes { get; set; }
    public DbSet<GroceryList> GroceryLists { get; set; }
    public DbSet<GroceryItem> GroceryItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        this.ConfigureEnumEntities(builder);
        this.ConfigureApplicationUser(builder);
        this.ConfigureMealPlan(builder);
        this.ConfigureMealDay(builder);
        this.ConfigureMeal(builder);
        this.ConfigureGroceryList(builder);
        this.ConfigureGroceryItem(builder);
    }
    
    private void ConfigureEnumEntities(ModelBuilder builder)
    {
        builder.Entity<ActivityLevelTypeEntity>().HasKey(e => e.Value);
        builder.Entity<ActivityLevelTypeEntity>().Property(e => e.Value).HasConversion<int>();
    
        foreach (var insertType in (ActivityLevels[])Enum.GetValues(typeof(ActivityLevels)))
            builder.Entity<ActivityLevelTypeEntity>().HasData(new ActivityLevelTypeEntity(insertType, insertType.ToString()));
    
        builder.Entity<DietTypeEntity>().HasKey(e => e.Value);
        builder.Entity<DietTypeEntity>().Property(e => e.Value).HasConversion<int>();
    
        foreach (var insertType in (DietType[])Enum.GetValues(typeof(DietType)))
            builder.Entity<DietTypeEntity>().HasData(new DietTypeEntity(insertType, insertType.ToString()));
    
        builder.Entity<GenderTypeEntity>().HasKey(e => e.Value);
        builder.Entity<GenderTypeEntity>().Property(e => e.Value).HasConversion<int>();
    
        foreach (var insertType in (Genders[])Enum.GetValues(typeof(Genders)))
            builder.Entity<GenderTypeEntity>().HasData(new GenderTypeEntity(insertType, insertType.ToString()));
        
        builder.Entity<MealTypeEntity>().HasKey(e => e.Value);
        builder.Entity<MealTypeEntity>().Property(e => e.Value).HasConversion<int>();
    
        foreach (var insertType in (MealTypes[])Enum.GetValues(typeof(MealTypes)))
            builder.Entity<MealTypeEntity>().HasData(new MealTypeEntity(insertType, insertType.ToString()));
    }
    
    private void ConfigureApplicationUser(ModelBuilder builder)
    {
        builder.Entity<ApplicationUser>()
            .Property(u => u.Gender).HasConversion<int>();
        
        builder.Entity<ApplicationUser>()
            .Property(u => u.ActivityLevels).HasConversion<int>();
        
        builder.Entity<ApplicationUser>()
            .HasOne(u => u.GenderRelation)
            .WithMany(g => g.ApplicationUsers)
            .HasForeignKey(u => u.Gender);

        builder.Entity<ApplicationUser>()
            .HasOne(u => u.ActivityLevelRelation)
            .WithMany(g => g.ApplicationUsers)
            .HasForeignKey(u => u.ActivityLevels);
    }

    private void ConfigureMealPlan(ModelBuilder builder)
    {
        builder.Entity<MealPlan>()
            .Property(u => u.DietType).HasConversion<int>();
        
        builder.Entity<MealPlan>()
            .HasOne(u => u.DietTypeRelation)
            .WithMany(g => g.MealPlans)
            .HasForeignKey(u => u.DietType);
            
        builder.Entity<MealPlan>()
            .HasMany(mp => mp.MealDays)
            .WithOne(md => md.MealPlan)
            .HasForeignKey(md => md.MealPlanId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<MealPlan>()
            .HasOne<ApplicationUser>(m => m.ApplicationUser)
            .WithMany(u => u.MealPlans)
            .HasForeignKey(u => u.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureMealDay(ModelBuilder builder)
    {
        builder.Entity<MealDay>()
            .HasMany(md => md.Meals)
            .WithOne(m => m.MealDay)
            .HasForeignKey(m => m.MealDayId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureMeal(ModelBuilder builder)
    {
        builder.Entity<Meal>()
            .Property(u => u.MealType).HasConversion<int>();
        
        builder.Entity<Meal>()
            .HasOne(u => u.MealTypeRelation)
            .WithMany(g => g.Meals)
            .HasForeignKey(u => u.MealType);
            
        builder.Entity<Meal>()
            .Property(m => m.Name)
            .IsRequired();

        builder.Entity<Meal>()
            .Property(m => m.Recipe)
            .IsRequired();
    }

    private void ConfigureGroceryList(ModelBuilder builder)
    {
        builder.Entity<GroceryList>()
            .HasOne(gl => gl.MealPlan)
            .WithOne(mp => mp.GroceryList)
            .HasForeignKey<GroceryList>(gl => gl.MealPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void ConfigureGroceryItem(ModelBuilder builder)
    {
        builder.Entity<GroceryItem>()
            .HasOne(gi => gi.GroceryList)
            .WithMany(gl => gl.Items)
            .HasForeignKey(gi => gi.GroceryListId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<GroceryItem>()
            .Property(gi => gi.Name)
            .IsRequired();
    }
}