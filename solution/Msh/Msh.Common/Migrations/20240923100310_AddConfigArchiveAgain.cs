using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Msh.Common.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigArchiveAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ConfigsArchive",
                columns: table => new
                {
                    ConfigType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Locked = table.Column<bool>(type: "bit", nullable: false),
                    Published = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublishedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigsArchive", x => x.ConfigType);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigsArchive");

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
    }
}
