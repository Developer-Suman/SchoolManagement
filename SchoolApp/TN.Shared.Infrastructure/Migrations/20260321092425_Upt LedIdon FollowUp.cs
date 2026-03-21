using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UptLedIdonFollowUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUps_Appointments_AppointmentId",
                table: "FollowUps");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "FollowUps",
                newName: "LeadId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowUps_AppointmentId",
                table: "FollowUps",
                newName: "IX_FollowUps_LeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUps_Leads_LeadId",
                table: "FollowUps",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUps_Leads_LeadId",
                table: "FollowUps");

            migrationBuilder.RenameColumn(
                name: "LeadId",
                table: "FollowUps",
                newName: "AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowUps_LeadId",
                table: "FollowUps",
                newName: "IX_FollowUps_AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUps_Appointments_AppointmentId",
                table: "FollowUps",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
