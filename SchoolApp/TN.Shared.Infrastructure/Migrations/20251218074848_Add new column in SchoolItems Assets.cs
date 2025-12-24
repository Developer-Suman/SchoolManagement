using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddnewcolumninSchoolItemsAssets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SchoolItemsHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SchoolItemsHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SchoolItemsHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "SchoolItemsHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "SchoolItemsHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SchoolId",
                table: "SchoolItemsHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SchoolItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SchoolItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "SchoolItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "SchoolItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SchoolId",
                table: "SchoolItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Contributors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Contributors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Contributors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Contributors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Contributors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SchoolId",
                table: "Contributors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SchoolItemsHistories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SchoolItemsHistories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SchoolItemsHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "SchoolItemsHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "SchoolItemsHistories");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "SchoolItemsHistories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SchoolItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SchoolItems");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "SchoolItems");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "SchoolItems");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "SchoolItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Contributors");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Contributors");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Contributors");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Contributors");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Contributors");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Contributors");
        }
    }
}
