using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiteAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMealOrderProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MealOrder",
                table: "Meals",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MealOrder",
                table: "Meals");
        }
    }
}
