using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class @as : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImgUrl",
                table: "Notices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImgUrl",
                table: "Notices",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
