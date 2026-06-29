using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposCliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Clientes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumeroCliente",
                table: "Clientes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pais",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaisOrigem",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "NumeroCliente",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Pais",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "PaisOrigem",
                table: "Clientes");
        }
    }
}
