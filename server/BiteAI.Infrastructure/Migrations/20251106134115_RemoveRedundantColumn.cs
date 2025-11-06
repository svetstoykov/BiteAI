using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiteAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRedundantColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastLoginAt",
                table: "MealPlans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLoginAt",
                table: "MealPlans",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
