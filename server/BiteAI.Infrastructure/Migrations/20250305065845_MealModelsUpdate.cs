using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiteAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MealModelsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MealPlans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Meals",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MealPlans",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
