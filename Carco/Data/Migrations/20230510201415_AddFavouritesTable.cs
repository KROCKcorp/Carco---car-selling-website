using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carco.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFavouritesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CarAdId = table.Column<int>(type: "int", nullable: false),
                    CarAdEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_CarAds_CarAdEntityId",
                        column: x => x.CarAdEntityId,
                        principalTable: "CarAds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favorites_CarAds_CarAdId",
                        column: x => x.CarAdId,
                        principalTable: "CarAds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_CarAdEntityId",
                table: "Favorites",
                column: "CarAdEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_CarAdId",
                table: "Favorites",
                column: "CarAdId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favorites");
        }
    }
}
