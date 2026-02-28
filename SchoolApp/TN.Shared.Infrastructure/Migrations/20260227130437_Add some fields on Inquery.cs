using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddsomefieldsonInquery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BandScore",
                table: "Leads",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnglishProficiency",
                table: "Leads",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstitutionName",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguageRemarks",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkillOrTrainingName",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingEndDate",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingRemarks",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingStartDate",
                table: "Leads",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BandScore",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "EnglishProficiency",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "InstitutionName",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "LanguageRemarks",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "SkillOrTrainingName",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "TrainingEndDate",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "TrainingRemarks",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "TrainingStartDate",
                table: "Leads");
        }
    }
}
