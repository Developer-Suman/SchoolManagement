using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addreationbetweencountryandrequirements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "Requirements",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_CountryId",
                table: "Requirements",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requirements_Countries_CountryId",
                table: "Requirements",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requirements_Countries_CountryId",
                table: "Requirements");

            migrationBuilder.DropIndex(
                name: "IX_Requirements_CountryId",
                table: "Requirements");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Requirements");
        }
    }
}
