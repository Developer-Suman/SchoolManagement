using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeschoolIdrelationinCertificat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CertificateTemplates_Schools_SchoolId",
                table: "CertificateTemplates");

            migrationBuilder.AddForeignKey(
                name: "FK_CertificateTemplates_Schools_SchoolId",
                table: "CertificateTemplates",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CertificateTemplates_Schools_SchoolId",
                table: "CertificateTemplates");

            migrationBuilder.AddForeignKey(
                name: "FK_CertificateTemplates_Schools_SchoolId",
                table: "CertificateTemplates",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
