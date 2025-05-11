using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackerAPIWebApp.Migrations
{
    /// <inheritdoc />
    public partial class BaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Users");

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "HealthRecords",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "HealthRecords");

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "Users",
                type: "real",
                nullable: true);
        }
    }
}
