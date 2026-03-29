using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddsomechangesonFollowUps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUps_Leads_LeadId",
                table: "FollowUps");

            migrationBuilder.RenameColumn(
                name: "LeadId",
                table: "FollowUps",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowUps_LeadId",
                table: "FollowUps",
                newName: "IX_FollowUps_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FollowUps_UserProfiles_UserId",
                table: "FollowUps",
                column: "UserId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FollowUps_UserProfiles_UserId",
                table: "FollowUps");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "FollowUps",
                newName: "LeadId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowUps_UserId",
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
    }
}
