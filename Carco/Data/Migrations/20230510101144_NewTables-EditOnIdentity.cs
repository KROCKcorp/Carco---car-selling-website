using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carco.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewTablesEditOnIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(600)",
                maxLength: 600,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UserBirthDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "CarAds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CarVIN = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: true),
                    CarYear = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    CarMake = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CarType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CarPower = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CarModel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CarKilometers = table.Column<int>(type: "int", maxLength: 9, nullable: false),
                    CarPrice = table.Column<int>(type: "int", maxLength: 15, nullable: false),
                    CarTransmission = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdDescription = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarAds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarAds_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarPictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarPhoto = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CarAdId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarPictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarPictures_CarAds_CarAdId",
                        column: x => x.CarAdId,
                        principalTable: "CarAds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarAds_UserId",
                table: "CarAds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarPictures_CarAdId",
                table: "CarPictures",
                column: "CarAdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarPictures");

            migrationBuilder.DropTable(
                name: "CarAds");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserBirthDate",
                table: "AspNetUsers");
        }
    }
}
