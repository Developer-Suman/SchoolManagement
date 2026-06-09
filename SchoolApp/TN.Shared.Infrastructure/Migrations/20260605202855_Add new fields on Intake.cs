using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddnewfieldsonIntake : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "Intakes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UniversityId",
                table: "Intakes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Intakes_CountryId",
                table: "Intakes",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Intakes_UniversityId",
                table: "Intakes",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Intakes_Countries_CountryId",
                table: "Intakes",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Intakes_Universities_UniversityId",
                table: "Intakes",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Intakes_Countries_CountryId",
                table: "Intakes");

            migrationBuilder.DropForeignKey(
                name: "FK_Intakes_Universities_UniversityId",
                table: "Intakes");

            migrationBuilder.DropIndex(
                name: "IX_Intakes_CountryId",
                table: "Intakes");

            migrationBuilder.DropIndex(
                name: "IX_Intakes_UniversityId",
                table: "Intakes");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Intakes");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "Intakes");
        }
    }
}
