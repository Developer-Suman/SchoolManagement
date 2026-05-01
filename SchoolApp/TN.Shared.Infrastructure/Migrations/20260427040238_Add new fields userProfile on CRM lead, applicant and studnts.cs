using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddnewfieldsuserProfileonCRMleadapplicantandstudnts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_UserProfiles_Id",
                table: "Applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmStudents_UserProfiles_Id",
                table: "CrmStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_UserProfiles_Id",
                table: "Leads");

            migrationBuilder.AddColumn<string>(
                name: "ProfileId",
                table: "Leads",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileId",
                table: "CrmStudents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileId",
                table: "Applicants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leads_ProfileId",
                table: "Leads",
                column: "ProfileId",
                unique: true,
                filter: "[ProfileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CrmStudents_ProfileId",
                table: "CrmStudents",
                column: "ProfileId",
                unique: true,
                filter: "[ProfileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_ProfileId",
                table: "Applicants",
                column: "ProfileId",
                unique: true,
                filter: "[ProfileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_UserProfiles_ProfileId",
                table: "Applicants",
                column: "ProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrmStudents_UserProfiles_ProfileId",
                table: "CrmStudents",
                column: "ProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_UserProfiles_ProfileId",
                table: "Leads",
                column: "ProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_UserProfiles_ProfileId",
                table: "Applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_CrmStudents_UserProfiles_ProfileId",
                table: "CrmStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_Leads_UserProfiles_ProfileId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_Leads_ProfileId",
                table: "Leads");

            migrationBuilder.DropIndex(
                name: "IX_CrmStudents_ProfileId",
                table: "CrmStudents");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_ProfileId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Leads");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "CrmStudents");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Applicants");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_UserProfiles_Id",
                table: "Applicants",
                column: "Id",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CrmStudents_UserProfiles_Id",
                table: "CrmStudents",
                column: "Id",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_UserProfiles_Id",
                table: "Leads",
                column: "Id",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
