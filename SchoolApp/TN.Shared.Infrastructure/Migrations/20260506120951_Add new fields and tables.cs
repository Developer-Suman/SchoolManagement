using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addnewfieldsandtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Appointmentid",
                table: "FollowUps",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FollowUps_Appointmentid",
                table: "FollowUps",
                column: "Appointmentid");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUps_Appointments_Appointmentid",
                table: "FollowUps",
                column: "Appointmentid",
                principalTable: "Appointments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUps_Appointments_Appointmentid",
                table: "FollowUps");

            migrationBuilder.DropIndex(
                name: "IX_FollowUps_Appointmentid",
                table: "FollowUps");

            migrationBuilder.DropColumn(
                name: "Appointmentid",
                table: "FollowUps");
        }
    }
}
