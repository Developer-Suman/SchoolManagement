using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyVisaApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "VisaApplicationStatusHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "VisaApplicationStatusHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FyId",
                table: "VisaApplicationStatusHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "VisaApplicationStatusHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "VisaApplicationStatusHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "VisaApplicationStatusHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SchoolId",
                table: "VisaApplicationStatusHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmailContent",
                table: "VisaApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EmailSent",
                table: "VisaApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VisaDetails",
                table: "VisaApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "VisaApplicationStatusHistories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "VisaApplicationStatusHistories");

            migrationBuilder.DropColumn(
                name: "FyId",
                table: "VisaApplicationStatusHistories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "VisaApplicationStatusHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "VisaApplicationStatusHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "VisaApplicationStatusHistories");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "VisaApplicationStatusHistories");

            migrationBuilder.DropColumn(
                name: "EmailContent",
                table: "VisaApplications");

            migrationBuilder.DropColumn(
                name: "EmailSent",
                table: "VisaApplications");

            migrationBuilder.DropColumn(
                name: "VisaDetails",
                table: "VisaApplications");
        }
    }
}
