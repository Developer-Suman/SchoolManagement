using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changenavigationofleadcountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadCountries_LeadCountries_CountryId",
                table: "LeadCountries");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadCountries_Leads_LeadId",
                table: "LeadCountries");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadCourses_LeadUniversities_LeadUniversityId",
                table: "LeadCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadUniversities_LeadCountries_LeadCountryId",
                table: "LeadUniversities");

            migrationBuilder.DropIndex(
                name: "IX_LeadCountries_CountryId",
                table: "LeadCountries");

            migrationBuilder.DropIndex(
                name: "IX_LeadCountries_LeadId",
                table: "LeadCountries");

            migrationBuilder.AlterColumn<string>(
                name: "LeadId",
                table: "LeadCountries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "LeadCountries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CrmLeadId",
                table: "LeadCountries",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeadCountries_CrmLeadId",
                table: "LeadCountries",
                column: "CrmLeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadCountries_Leads_CrmLeadId",
                table: "LeadCountries",
                column: "CrmLeadId",
                principalTable: "Leads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadCourses_LeadUniversities_LeadUniversityId",
                table: "LeadCourses",
                column: "LeadUniversityId",
                principalTable: "LeadUniversities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadUniversities_LeadCountries_LeadCountryId",
                table: "LeadUniversities",
                column: "LeadCountryId",
                principalTable: "LeadCountries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadCountries_Leads_CrmLeadId",
                table: "LeadCountries");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadCourses_LeadUniversities_LeadUniversityId",
                table: "LeadCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_LeadUniversities_LeadCountries_LeadCountryId",
                table: "LeadUniversities");

            migrationBuilder.DropIndex(
                name: "IX_LeadCountries_CrmLeadId",
                table: "LeadCountries");

            migrationBuilder.DropColumn(
                name: "CrmLeadId",
                table: "LeadCountries");

            migrationBuilder.AlterColumn<string>(
                name: "LeadId",
                table: "LeadCountries",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountryId",
                table: "LeadCountries",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeadCountries_CountryId",
                table: "LeadCountries",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadCountries_LeadId",
                table: "LeadCountries",
                column: "LeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadCountries_LeadCountries_CountryId",
                table: "LeadCountries",
                column: "CountryId",
                principalTable: "LeadCountries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadCountries_Leads_LeadId",
                table: "LeadCountries",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadCourses_LeadUniversities_LeadUniversityId",
                table: "LeadCourses",
                column: "LeadUniversityId",
                principalTable: "LeadUniversities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeadUniversities_LeadCountries_LeadCountryId",
                table: "LeadUniversities",
                column: "LeadCountryId",
                principalTable: "LeadCountries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
