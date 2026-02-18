using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddnewfieldsonSchoolSettingsnamedAcademicYearId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcademicYearId",
                table: "SchoolSettings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSettings_AcademicYearId",
                table: "SchoolSettings",
                column: "AcademicYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolSettings_AcademicYears_AcademicYearId",
                table: "SchoolSettings",
                column: "AcademicYearId",
                principalTable: "AcademicYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolSettings_AcademicYears_AcademicYearId",
                table: "SchoolSettings");

            migrationBuilder.DropIndex(
                name: "IX_SchoolSettings_AcademicYearId",
                table: "SchoolSettings");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "SchoolSettings");
        }
    }
}
