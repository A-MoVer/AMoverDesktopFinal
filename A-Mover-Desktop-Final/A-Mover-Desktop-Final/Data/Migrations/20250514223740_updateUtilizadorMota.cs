using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateUtilizadorMota : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataInativacao",
                table: "UtilizadorMota",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "UtilizadorMota",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MotivoInativacao",
                table: "UtilizadorMota",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataInativacao",
                table: "UtilizadorMota");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "UtilizadorMota");

            migrationBuilder.DropColumn(
                name: "MotivoInativacao",
                table: "UtilizadorMota");
        }
    }
}
