using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BiteAI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ActivityLevels_ActivityLevels",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Genders_Gender",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ActivityLevels",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ActivityLevels",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HeightInCm",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WeightInKg",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "AspNetUsers",
                newName: "GenderTypeEntityValue");

            migrationBuilder.RenameColumn(
                name: "Age",
                table: "AspNetUsers",
                newName: "ActivityLevelTypeEntityValue");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Gender",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_GenderTypeEntityValue");

            migrationBuilder.CreateTable(
                name: "AllergyTypes",
                columns: table => new
                {
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllergyTypes", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "FoodDislikeTypes",
                columns: table => new
                {
                    Value = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodDislikeTypes", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: true),
                    WeightInKg = table.Column<double>(type: "double precision", nullable: true),
                    HeightInCm = table.Column<double>(type: "double precision", nullable: true),
                    ActivityLevel = table.Column<int>(type: "integer", nullable: true),
                    PreferredDietType = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsProfileComplete = table.Column<bool>(type: "boolean", nullable: false),
                    OnboardingCompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_ActivityLevels_ActivityLevel",
                        column: x => x.ActivityLevel,
                        principalTable: "ActivityLevels",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfiles_DietTypes_PreferredDietType",
                        column: x => x.PreferredDietType,
                        principalTable: "DietTypes",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Genders_Gender",
                        column: x => x.Gender,
                        principalTable: "Genders",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProfileAllergies",
                columns: table => new
                {
                    AllergiesValue = table.Column<int>(type: "integer", nullable: false),
                    UserProfilesId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileAllergies", x => new { x.AllergiesValue, x.UserProfilesId });
                    table.ForeignKey(
                        name: "FK_UserProfileAllergies_AllergyTypes_AllergiesValue",
                        column: x => x.AllergiesValue,
                        principalTable: "AllergyTypes",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfileAllergies_UserProfiles_UserProfilesId",
                        column: x => x.UserProfilesId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfileFoodDislikes",
                columns: table => new
                {
                    FoodDislikesValue = table.Column<int>(type: "integer", nullable: false),
                    UserProfilesId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileFoodDislikes", x => new { x.FoodDislikesValue, x.UserProfilesId });
                    table.ForeignKey(
                        name: "FK_UserProfileFoodDislikes_FoodDislikeTypes_FoodDislikesValue",
                        column: x => x.FoodDislikesValue,
                        principalTable: "FoodDislikeTypes",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfileFoodDislikes_UserProfiles_UserProfilesId",
                        column: x => x.UserProfilesId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AllergyTypes",
                columns: new[] { "Value", "Name" },
                values: new object[,]
                {
                    { 0, "None" },
                    { 1, "Peanuts" },
                    { 2, "TreeNuts" },
                    { 3, "Dairy" },
                    { 4, "Eggs" },
                    { 5, "Soy" },
                    { 6, "Wheat" },
                    { 7, "Gluten" },
                    { 8, "Fish" },
                    { 9, "Shellfish" },
                    { 10, "Sesame" },
                    { 11, "Mustard" },
                    { 12, "Celery" },
                    { 13, "Sulphites" },
                    { 14, "Lupin" },
                    { 15, "Molluscs" },
                    { 16, "Crustaceans" },
                    { 17, "Milk" },
                    { 18, "Cheese" },
                    { 19, "Yogurt" },
                    { 20, "Butter" },
                    { 21, "Almonds" },
                    { 22, "Walnuts" },
                    { 23, "Cashews" },
                    { 24, "Pistachios" },
                    { 25, "Hazelnuts" },
                    { 26, "BrazilNuts" },
                    { 27, "MacadamiaNuts" },
                    { 28, "Pecans" },
                    { 29, "PineNuts" },
                    { 30, "Coconut" },
                    { 31, "Kiwi" },
                    { 32, "Banana" },
                    { 33, "Avocado" },
                    { 34, "Pineapple" },
                    { 35, "Mango" },
                    { 36, "Papaya" },
                    { 37, "PassionFruit" },
                    { 38, "Guava" },
                    { 39, "DragonFruit" },
                    { 40, "Durian" },
                    { 41, "Jackfruit" },
                    { 42, "Rambutan" },
                    { 43, "Lychee" },
                    { 44, "Longan" },
                    { 45, "Mangosteen" },
                    { 46, "Salak" },
                    { 47, "SnakeFruit" },
                    { 48, "CustardApple" },
                    { 49, "Soursop" },
                    { 50, "Atemoya" }
                });

            migrationBuilder.InsertData(
                table: "FoodDislikeTypes",
                columns: new[] { "Value", "Name" },
                values: new object[,]
                {
                    { 0, "None" },
                    { 1, "Broccoli" },
                    { 2, "BrusselsSprouts" },
                    { 3, "Cauliflower" },
                    { 4, "Cabbage" },
                    { 5, "Kale" },
                    { 6, "Spinach" },
                    { 7, "Asparagus" },
                    { 8, "Artichokes" },
                    { 9, "Beets" },
                    { 10, "Radishes" },
                    { 11, "Turnips" },
                    { 12, "Parsnips" },
                    { 13, "Rutabagas" },
                    { 14, "Celery" },
                    { 15, "Fennel" },
                    { 16, "Leeks" },
                    { 17, "Onions" },
                    { 18, "Garlic" },
                    { 19, "Shallots" },
                    { 20, "Scallions" },
                    { 21, "Chives" },
                    { 22, "Mushrooms" },
                    { 23, "Eggplant" },
                    { 24, "Zucchini" },
                    { 25, "Squash" },
                    { 26, "Cucumber" },
                    { 27, "BellPeppers" },
                    { 28, "ChiliPeppers" },
                    { 29, "Jalapenos" },
                    { 30, "Okra" },
                    { 31, "GreenBeans" },
                    { 32, "Peas" },
                    { 33, "LimaBeans" },
                    { 34, "KidneyBeans" },
                    { 35, "BlackBeans" },
                    { 36, "PintoBeans" },
                    { 37, "Chickpeas" },
                    { 38, "Lentils" },
                    { 39, "Tofu" },
                    { 40, "Tempeh" },
                    { 41, "Seitan" },
                    { 42, "Quinoa" },
                    { 43, "Couscous" },
                    { 44, "Bulgur" },
                    { 45, "Farro" },
                    { 46, "Barley" },
                    { 47, "Rye" },
                    { 48, "Oats" },
                    { 49, "Millet" },
                    { 50, "Amaranth" },
                    { 51, "Buckwheat" },
                    { 52, "Sorghum" },
                    { 53, "Teff" },
                    { 54, "Spelt" },
                    { 55, "Kamut" },
                    { 56, "Einkorn" },
                    { 57, "Emmer" },
                    { 58, "Freekeh" },
                    { 59, "WildRice" },
                    { 60, "BrownRice" },
                    { 61, "WhiteRice" },
                    { 62, "BasmatiRice" },
                    { 63, "JasmineRice" },
                    { 64, "ArborioRice" },
                    { 65, "SushiRice" },
                    { 66, "BlackRice" },
                    { 67, "RedRice" },
                    { 68, "GlutinousRice" },
                    { 69, "ParboiledRice" },
                    { 70, "ConvertedRice" },
                    { 71, "Beef" },
                    { 72, "Pork" },
                    { 73, "Chicken" },
                    { 74, "Turkey" },
                    { 75, "Duck" },
                    { 76, "Goose" },
                    { 77, "Lamb" },
                    { 78, "Veal" },
                    { 79, "Rabbit" },
                    { 80, "Venison" },
                    { 81, "Bison" },
                    { 82, "Elk" },
                    { 83, "Ostrich" },
                    { 84, "Emu" },
                    { 85, "Quail" },
                    { 86, "Pheasant" },
                    { 87, "CornishHen" },
                    { 88, "Squab" },
                    { 89, "Salmon" },
                    { 90, "Tuna" },
                    { 91, "Cod" },
                    { 92, "Halibut" },
                    { 93, "Swordfish" },
                    { 94, "MahiMahi" },
                    { 95, "Grouper" },
                    { 96, "Snapper" },
                    { 97, "Flounder" },
                    { 98, "Sole" },
                    { 99, "Trout" },
                    { 100, "Catfish" },
                    { 101, "Tilapia" },
                    { 102, "Shrimp" },
                    { 103, "Crab" },
                    { 104, "Lobster" },
                    { 105, "Clams" },
                    { 106, "Mussels" },
                    { 107, "Oysters" },
                    { 108, "Scallops" },
                    { 109, "Squid" },
                    { 110, "Octopus" },
                    { 111, "Calamari" },
                    { 112, "SeaUrchin" },
                    { 113, "Roe" },
                    { 114, "Caviar" },
                    { 115, "Anchovies" },
                    { 116, "Sardines" },
                    { 117, "Herring" },
                    { 118, "Mackerel" },
                    { 119, "Bluefish" },
                    { 120, "Monkfish" },
                    { 121, "Skate" },
                    { 122, "Eel" },
                    { 123, "Conch" },
                    { 124, "Abalone" },
                    { 125, "Milk" },
                    { 126, "Cheese" },
                    { 127, "Yogurt" },
                    { 128, "Butter" },
                    { 129, "Cream" },
                    { 130, "IceCream" },
                    { 131, "CottageCheese" },
                    { 132, "Ricotta" },
                    { 133, "Mozzarella" },
                    { 134, "Cheddar" },
                    { 135, "Parmesan" },
                    { 136, "Gouda" },
                    { 137, "Brie" },
                    { 138, "Camembert" },
                    { 139, "Roquefort" },
                    { 140, "Feta" },
                    { 141, "GoatCheese" },
                    { 142, "BlueCheese" },
                    { 143, "Swiss" },
                    { 144, "Provolone" },
                    { 145, "MontereyJack" },
                    { 146, "Colby" },
                    { 147, "American" },
                    { 148, "Velveeta" },
                    { 149, "Eggs" },
                    { 150, "Mayonnaise" },
                    { 151, "Aioli" },
                    { 152, "TartarSauce" },
                    { 153, "Hollandaise" },
                    { 154, "Bearnaise" },
                    { 155, "CaesarDressing" },
                    { 156, "RanchDressing" },
                    { 157, "ThousandIsland" },
                    { 158, "ItalianDressing" },
                    { 159, "FrenchDressing" },
                    { 160, "BalsamicVinaigrette" },
                    { 161, "OilAndVinegar" },
                    { 162, "HoneyMustard" },
                    { 163, "BlueCheeseDressing" },
                    { 164, "RussianDressing" },
                    { 165, "SesameGinger" },
                    { 166, "Teriyaki" },
                    { 167, "SoySauce" },
                    { 168, "Worcestershire" },
                    { 169, "HotSauce" },
                    { 170, "Sriracha" },
                    { 171, "Tabasco" },
                    { 172, "BuffaloSauce" },
                    { 173, "BBQ" },
                    { 174, "Ketchup" },
                    { 175, "Mustard" },
                    { 176, "Relish" },
                    { 177, "Pickles" },
                    { 178, "Olives" },
                    { 179, "Capers" },
                    { 180, "Horseradish" },
                    { 181, "Wasabi" },
                    { 182, "Apples" },
                    { 183, "Bananas" },
                    { 184, "Oranges" },
                    { 185, "Grapes" },
                    { 186, "Strawberries" },
                    { 187, "Blueberries" },
                    { 188, "Raspberries" },
                    { 189, "Blackberries" },
                    { 190, "Cranberries" },
                    { 191, "Pineapple" },
                    { 192, "Mango" },
                    { 193, "Papaya" },
                    { 194, "Kiwi" },
                    { 195, "PassionFruit" },
                    { 196, "Guava" },
                    { 197, "DragonFruit" },
                    { 198, "Durian" },
                    { 199, "Jackfruit" },
                    { 200, "Rambutan" },
                    { 201, "Lychee" },
                    { 202, "Longan" },
                    { 203, "Mangosteen" },
                    { 204, "Salak" },
                    { 205, "SnakeFruit" },
                    { 206, "CustardApple" },
                    { 207, "Soursop" },
                    { 208, "Atemoya" },
                    { 209, "Cherries" },
                    { 210, "Peaches" },
                    { 211, "Plums" },
                    { 212, "Apricots" },
                    { 213, "Nectarines" },
                    { 214, "Pears" },
                    { 215, "Lemons" },
                    { 216, "Limes" },
                    { 217, "Grapefruit" },
                    { 218, "Tangerines" },
                    { 219, "Clementines" },
                    { 220, "Mandarins" },
                    { 221, "Pomelo" },
                    { 222, "Kumquats" },
                    { 223, "Calamansi" },
                    { 224, "Yuzu" },
                    { 225, "BuddhaHand" },
                    { 226, "FingerLimes" },
                    { 227, "Chocolate" },
                    { 228, "DarkChocolate" },
                    { 229, "MilkChocolate" },
                    { 230, "WhiteChocolate" },
                    { 231, "Cocoa" },
                    { 232, "Carob" },
                    { 233, "Licorice" },
                    { 234, "Peppermint" },
                    { 235, "Spearmint" },
                    { 236, "Cinnamon" },
                    { 237, "Nutmeg" },
                    { 238, "Cloves" },
                    { 239, "Allspice" },
                    { 240, "Cardamom" },
                    { 241, "Coriander" },
                    { 242, "Cumin" },
                    { 243, "FennelSeeds" },
                    { 244, "Fenugreek" },
                    { 245, "MustardSeeds" },
                    { 246, "PoppySeeds" },
                    { 247, "SesameSeeds" },
                    { 248, "SunflowerSeeds" },
                    { 249, "PumpkinSeeds" },
                    { 250, "ChiaSeeds" },
                    { 251, "Flaxseeds" },
                    { 252, "HempSeeds" },
                    { 253, "PineNuts" },
                    { 254, "Almonds" },
                    { 255, "Walnuts" },
                    { 256, "Cashews" },
                    { 257, "Pistachios" },
                    { 258, "Hazelnuts" },
                    { 259, "BrazilNuts" },
                    { 260, "MacadamiaNuts" },
                    { 261, "Pecans" },
                    { 262, "Chestnuts" },
                    { 263, "Coconut" },
                    { 264, "Peanuts" },
                    { 265, "Salt" },
                    { 266, "Pepper" },
                    { 267, "Paprika" },
                    { 268, "ChiliPowder" },
                    { 269, "CurryPowder" },
                    { 270, "GaramMasala" },
                    { 271, "TacoSeasoning" },
                    { 272, "ItalianSeasoning" },
                    { 273, "HerbsDeProvence" },
                    { 274, "PoultrySeasoning" },
                    { 275, "PumpkinPieSpice" },
                    { 276, "ApplePieSpice" },
                    { 277, "ChineseFiveSpice" },
                    { 278, "CajunSeasoning" },
                    { 279, "CreoleSeasoning" },
                    { 280, "OldBay" },
                    { 281, "LemonPepper" },
                    { 282, "GarlicPowder" },
                    { 283, "OnionPowder" },
                    { 284, "Thyme" },
                    { 285, "Rosemary" },
                    { 286, "Sage" },
                    { 287, "Oregano" },
                    { 288, "Basil" },
                    { 289, "Parsley" },
                    { 290, "Cilantro" },
                    { 291, "Dill" },
                    { 292, "Tarragon" },
                    { 293, "Chervil" },
                    { 294, "Sorrel" },
                    { 295, "Borage" },
                    { 296, "Lovage" },
                    { 297, "SummerSavory" },
                    { 298, "WinterSavory" },
                    { 299, "Marjoram" },
                    { 300, "Hyssop" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ActivityLevelTypeEntityValue",
                table: "AspNetUsers",
                column: "ActivityLevelTypeEntityValue");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileAllergies_UserProfilesId",
                table: "UserProfileAllergies",
                column: "UserProfilesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileFoodDislikes_UserProfilesId",
                table: "UserProfileFoodDislikes",
                column: "UserProfilesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ActivityLevel",
                table: "UserProfiles",
                column: "ActivityLevel");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CreatedAt",
                table: "UserProfiles",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_Gender",
                table: "UserProfiles",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_IsProfileComplete",
                table: "UserProfiles",
                column: "IsProfileComplete");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_PreferredDietType",
                table: "UserProfiles",
                column: "PreferredDietType");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ActivityLevels_ActivityLevelTypeEntityValue",
                table: "AspNetUsers",
                column: "ActivityLevelTypeEntityValue",
                principalTable: "ActivityLevels",
                principalColumn: "Value");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Genders_GenderTypeEntityValue",
                table: "AspNetUsers",
                column: "GenderTypeEntityValue",
                principalTable: "Genders",
                principalColumn: "Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ActivityLevels_ActivityLevelTypeEntityValue",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Genders_GenderTypeEntityValue",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserProfileAllergies");

            migrationBuilder.DropTable(
                name: "UserProfileFoodDislikes");

            migrationBuilder.DropTable(
                name: "AllergyTypes");

            migrationBuilder.DropTable(
                name: "FoodDislikeTypes");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ActivityLevelTypeEntityValue",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "GenderTypeEntityValue",
                table: "AspNetUsers",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "ActivityLevelTypeEntityValue",
                table: "AspNetUsers",
                newName: "Age");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_GenderTypeEntityValue",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Gender");

            migrationBuilder.AddColumn<int>(
                name: "ActivityLevels",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HeightInCm",
                table: "AspNetUsers",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WeightInKg",
                table: "AspNetUsers",
                type: "double precision",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ActivityLevels",
                table: "AspNetUsers",
                column: "ActivityLevels");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ActivityLevels_ActivityLevels",
                table: "AspNetUsers",
                column: "ActivityLevels",
                principalTable: "ActivityLevels",
                principalColumn: "Value");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Genders_Gender",
                table: "AspNetUsers",
                column: "Gender",
                principalTable: "Genders",
                principalColumn: "Value");
        }
    }
}
