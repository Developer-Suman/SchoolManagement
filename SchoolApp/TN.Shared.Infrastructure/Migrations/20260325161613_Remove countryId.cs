using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    public partial class RemovecountryId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Use raw SQL to safely drop FK if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Requirements_Countries_CountryId')
                BEGIN
                    ALTER TABLE Requirements DROP CONSTRAINT FK_Requirements_Countries_CountryId
                END
            ");

            // Drop the index if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Requirements_CountryId' AND object_id = OBJECT_ID('Requirements'))
                BEGIN
                    DROP INDEX IX_Requirements_CountryId ON Requirements
                END
            ");

            // Drop the column if it exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.columns 
                           WHERE Name = N'CountryId' AND Object_ID = Object_ID(N'Requirements'))
                BEGIN
                    ALTER TABLE Requirements DROP COLUMN CountryId
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate the column
            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "Requirements",
                type: "nvarchar(450)",
                nullable: true);

            // Recreate index
            migrationBuilder.CreateIndex(
                name: "IX_Requirements_CountryId",
                table: "Requirements",
                column: "CountryId");

            // Recreate foreign key
            migrationBuilder.AddForeignKey(
                name: "FK_Requirements_Countries_CountryId",
                table: "Requirements",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}