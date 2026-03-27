using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class llllll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key if it exists
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Requirements_Countries_CountryId')
        BEGIN
            ALTER TABLE Requirements DROP CONSTRAINT FK_Requirements_Countries_CountryId
        END
    ");

            // Drop index if it exists
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Requirements_CountryId' AND object_id = OBJECT_ID('Requirements'))
        BEGIN
            DROP INDEX IX_Requirements_CountryId ON Requirements
        END
    ");

            // Drop column if it exists
            migrationBuilder.Sql(@"
        IF EXISTS (SELECT * FROM sys.columns 
                   WHERE Name = N'CountryId' AND Object_ID = Object_ID(N'Requirements'))
        BEGIN
            ALTER TABLE Requirements DROP COLUMN CountryId
        END
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "Requirements",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_CountryId",
                table: "Requirements",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requirements_Countries_CountryId",
                table: "Requirements",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");
        }
    }
}
