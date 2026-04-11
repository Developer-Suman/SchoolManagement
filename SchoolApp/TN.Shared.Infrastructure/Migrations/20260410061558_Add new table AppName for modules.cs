using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddnewtableAppNameformodules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppId",
                table: "Modules",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppNames",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppNames", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Modules_AppId",
                table: "Modules",
                column: "AppId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_AppNames_AppId",
                table: "Modules",
                column: "AppId",
                principalTable: "AppNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_AppNames_AppId",
                table: "Modules");

            migrationBuilder.DropTable(
                name: "AppNames");

            migrationBuilder.DropIndex(
                name: "IX_Modules_AppId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "AppId",
                table: "Modules");
        }
    }
}
