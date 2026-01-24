using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateMotaQr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "QrCriadoEm",
                table: "Motas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QrToken",
                table: "Motas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QrCriadoEm",
                table: "Motas");

            migrationBuilder.DropColumn(
                name: "QrToken",
                table: "Motas");
        }
    }
}
