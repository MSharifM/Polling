using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Polling.Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_updateOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Options",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Options");
        }
    }
}
