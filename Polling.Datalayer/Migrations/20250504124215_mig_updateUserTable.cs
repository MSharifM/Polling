using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Polling.Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_updateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "FullName");

            migrationBuilder.AddColumn<string>(
                name: "StudentCode",
                table: "Users",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentCode",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Users",
                newName: "UserName");
        }
    }
}
