using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Msh.Common.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Locked",
                table: "Configs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Published",
                table: "Configs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PublishedBy",
                table: "Configs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Locked",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "Published",
                table: "Configs");

            migrationBuilder.DropColumn(
                name: "PublishedBy",
                table: "Configs");
        }
    }
}
