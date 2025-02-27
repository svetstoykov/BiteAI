﻿// <auto-generated />
using System;
using BiteAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BiteAI.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250227080019_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BiteAI.Infrastructure.Models.IdentityAccount", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastLoginAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("BiteAI.Services.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int?>("ActivityLevels")
                        .HasColumnType("integer");

                    b.Property<int?>("Age")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Gender")
                        .HasColumnType("integer");

                    b.Property<double?>("HeightInCm")
                        .HasColumnType("double precision");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double?>("WeightInKg")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("ActivityLevels");

                    b.HasIndex("Gender");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("BiteAI.Services.Entities.Enums.ActivityLevelTypeEntity", b =>
                {
                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Value");

                    b.ToTable("ActivityLevels");

                    b.HasData(
                        new
                        {
                            Value = 0,
                            Name = "NotSpecified"
                        },
                        new
                        {
                            Value = 1,
                            Name = "Sedentary"
                        },
                        new
                        {
                            Value = 2,
                            Name = "LightlyActive"
                        },
                        new
                        {
                            Value = 3,
                            Name = "ModeratelyActive"
                        },
                        new
                        {
                            Value = 4,
                            Name = "VeryActive"
                        },
                        new
                        {
                            Value = 5,
                            Name = "ExtraActive"
                        });
                });

            modelBuilder.Entity("BiteAI.Services.Entities.Enums.DietTypeEntity", b =>
                {
                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Value");

                    b.ToTable("DietTypes");

                    b.HasData(
                        new
                        {
                            Value = 0,
                            Name = "Standard"
                        },
                        new
                        {
                            Value = 1,
                            Name = "Vegetarian"
                        },
                        new
                        {
                            Value = 2,
                            Name = "Vegan"
                        },
                        new
                        {
                            Value = 3,
                            Name = "Keto"
                        },
                        new
                        {
                            Value = 4,
                            Name = "LowCarb"
                        },
                        new
                        {
                            Value = 5,
                            Name = "Paleo"
                        },
                        new
                        {
                            Value = 6,
                            Name = "Mediterranean"
                        },
                        new
                        {
                            Value = 7,
                            Name = "GlutenFree"
                        },
                        new
                        {
                            Value = 8,
                            Name = "DairyFree"
                        });
                });

            modelBuilder.Entity("BiteAI.Services.Entities.Enums.GenderTypeEntity", b =>
                {
                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Value");

                    b.ToTable("Genders");

                    b.HasData(
                        new
                        {
                            Value = 1,
                            Name = "Male"
                        },
                        new
                        {
                            Value = 2,
                            Name = "Female"
                        });
                });

            modelBuilder.Entity("BiteAI.Services.Entities.Enums.MealTypeEntity", b =>
                {
                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Value");

                    b.ToTable("MealTypes");

                    b.HasData(
                        new
                        {
                            Value = 0,
                            Name = "Breakfast"
                        },
                        new
                        {
                            Value = 1,
                            Name = "Lunch"
                        },
                        new
                        {
                            Value = 2,
                            Name = "Dinner"
                        },
                        new
                        {
                            Value = 3,
                            Name = "Snack"
                        });
                });

            modelBuilder.Entity("BiteAI.Services.Entities.Meal", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("Calories")
                        .HasColumnType("integer");

                    b.Property<int>("CarbsInGrams")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("FatInGrams")
                        .HasColumnType("integer");

                    b.Property<string>("MealDayId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MealType")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProteinInGrams")
                        .HasColumnType("integer");

                    b.Property<string>("Recipe")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MealDayId");

                    b.HasIndex("MealType");

                    b.ToTable("Meals");
                });

            modelBuilder.Entity("BiteAI.Services.Entities.MealDay", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DayNumber")
                        .HasColumnType("integer");

                    b.Property<string>("MealPlanId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TotalCalories")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MealPlanId");

                    b.ToTable("MealDays");
                });

            modelBuilder.Entity("BiteAI.Services.Entities.MealPlan", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ApplicationUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DailyCalories")
                        .HasColumnType("integer");

                    b.Property<int>("DietType")
                        .HasColumnType("integer");

                    b.Property<int>("DurationDays")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("DietType");

                    b.HasIndex("UserId");

                    b.ToTable("MealPlans");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("BiteAI.Infrastructure.Models.IdentityAccount", b =>
                {
                    b.HasOne("BiteAI.Services.Entities.ApplicationUser", "ApplicationUser")
                        .WithOne()
                        .HasForeignKey("BiteAI.Infrastructure.Models.IdentityAccount", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("BiteAI.Services.Entities.ApplicationUser", b =>
                {
                    b.HasOne("BiteAI.Services.Entities.Enums.ActivityLevelTypeEntity", "ActivityLevelRelation")
                        .WithMany("ApplicationUsers")
                        .HasForeignKey("ActivityLevels");

                    b.HasOne("BiteAI.Services.Entities.Enums.GenderTypeEntity", "GenderRelation")
                        .WithMany("ApplicationUsers")
                        .HasForeignKey("Gender");

                    b.Navigation("ActivityLevelRelation");

                    b.Navigation("GenderRelation");
                });

            modelBuilder.Entity("BiteAI.Services.Entities.Meal", b =>
                {
                    b.HasOne("BiteAI.Services.Entities.MealDay", "MealDay")
                        .WithMany("Meals")
                        .HasForeignKey("MealDayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BiteAI.Services.Entities.Enums.MealTypeEntity", "MealTypeRelation")
                        .WithMany("Meals")
                        .HasForeignKey("MealType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MealDay");

                    b.Navigation("MealTypeRelation");
                });

            modelBuilder.Entity("BiteAI.Services.Entities.MealDay", b =>
                {
                    b.HasOne("BiteAI.Services.Entities.MealPlan", "MealPlan")
                        .WithMany("MealDays")
                        .HasForeignKey("MealPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MealPlan");
                });

            modelBuilder.Entity("BiteAI.Services.Entities.MealPlan", b =>
                {
                    b.HasOne("BiteAI.Services.Entities.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BiteAI.Services.Entities.Enums.DietTypeEntity", "DietTypeRelation")
                        .WithMany("MealPlans")
                        .HasForeignKey("DietType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BiteAI.Services.Entities.ApplicationUser", null)
                        .WithMany("MealPlans")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("DietTypeRelation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BiteAI.Infrastructure.Models.IdentityAccount", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BiteAI.Infrastructure.Models.IdentityAccount", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BiteAI.Infrastructure.Models.IdentityAccount", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BiteAI.Infrastructure.Models.IdentityAccount", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BiteAI.Services.Entities.ApplicationUser", b =>
                {
                    b.Navigation("MealPlans");
                });

            modelBuilder.Entity("BiteAI.Services.Entities.Enums.ActivityLevelTypeEntity", b =>
                {
                    b.Navigation("ApplicationUsers");
                });

            modelBuilder.Entity("BiteAI.Services.Entities.Enums.DietTypeEntity", b =>
                {
                    b.Navigation("MealPlans");
                });

            modelBuilder.Entity("BiteAI.Services.Entities.Enums.GenderTypeEntity", b =>
                {
                    b.Navigation("ApplicationUsers");
                });

            modelBuilder.Entity("BiteAI.Services.Entities.Enums.MealTypeEntity", b =>
                {
                    b.Navigation("Meals");
                });

            modelBuilder.Entity("BiteAI.Services.Entities.MealDay", b =>
                {
                    b.Navigation("Meals");
                });

            modelBuilder.Entity("BiteAI.Services.Entities.MealPlan", b =>
                {
                    b.Navigation("MealDays");
                });
#pragma warning restore 612, 618
        }
    }
}
