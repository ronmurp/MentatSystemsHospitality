using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Msh.Common.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfigsPub",
                table: "ConfigsPub");

            migrationBuilder.RenameTable(
                name: "ConfigsPub",
                newName: "ConfigPub");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfigPub",
                table: "ConfigPub",
                column: "ConfigType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfigPub",
                table: "ConfigPub");

            migrationBuilder.RenameTable(
                name: "ConfigPub",
                newName: "ConfigsPub");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfigsPub",
                table: "ConfigsPub",
                column: "ConfigType");
        }
    }
}
