using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CertificateTemplateId",
                table: "StudentsAwards",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentHtml",
                table: "StudentsAwards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Eventsid",
                table: "StudentsAwards",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FyId",
                table: "StudentsAwards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsAwards_CertificateTemplateId",
                table: "StudentsAwards",
                column: "CertificateTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsAwards_Eventsid",
                table: "StudentsAwards",
                column: "Eventsid");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsAwards_CertificateTemplates_CertificateTemplateId",
                table: "StudentsAwards",
                column: "CertificateTemplateId",
                principalTable: "CertificateTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsAwards_Events_Eventsid",
                table: "StudentsAwards",
                column: "Eventsid",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentsAwards_CertificateTemplates_CertificateTemplateId",
                table: "StudentsAwards");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentsAwards_Events_Eventsid",
                table: "StudentsAwards");

            migrationBuilder.DropIndex(
                name: "IX_StudentsAwards_CertificateTemplateId",
                table: "StudentsAwards");

            migrationBuilder.DropIndex(
                name: "IX_StudentsAwards_Eventsid",
                table: "StudentsAwards");

            migrationBuilder.DropColumn(
                name: "CertificateTemplateId",
                table: "StudentsAwards");

            migrationBuilder.DropColumn(
                name: "ContentHtml",
                table: "StudentsAwards");

            migrationBuilder.DropColumn(
                name: "Eventsid",
                table: "StudentsAwards");

            migrationBuilder.DropColumn(
                name: "FyId",
                table: "StudentsAwards");
        }
    }
}
