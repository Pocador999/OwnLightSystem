using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FeatToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JwtToken",
                table: "Routines",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JwtToken",
                table: "Routines");
        }
    }
}
