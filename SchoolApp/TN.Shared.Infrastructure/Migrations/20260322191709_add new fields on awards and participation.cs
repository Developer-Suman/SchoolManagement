using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addnewfieldsonawardsandparticipation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwardTitle",
                table: "StudentsAwards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificateContent",
                table: "Participations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificateTitle",
                table: "Participations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwardTitle",
                table: "StudentsAwards");

            migrationBuilder.DropColumn(
                name: "CertificateContent",
                table: "Participations");

            migrationBuilder.DropColumn(
                name: "CertificateTitle",
                table: "Participations");
        }
    }
}
