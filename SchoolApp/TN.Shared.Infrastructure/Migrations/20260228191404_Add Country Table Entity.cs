using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryTableEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Universities");

            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "Universities",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Universities_CountryId",
                table: "Universities",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Universities_Countries_CountryId",
                table: "Universities",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Universities_Countries_CountryId",
                table: "Universities");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Universities_CountryId",
                table: "Universities");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Universities");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Universities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
