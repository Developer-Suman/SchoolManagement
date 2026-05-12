using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangesdateTimefiedsinEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EventsDate",
                table: "Events",
                newName: "ToDate");

            migrationBuilder.AddColumn<string>(
                name: "FromDate",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "ToDate",
                table: "Events",
                newName: "EventsDate");
        }
    }
}
