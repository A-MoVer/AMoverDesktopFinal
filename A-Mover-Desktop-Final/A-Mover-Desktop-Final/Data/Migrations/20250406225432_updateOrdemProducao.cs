using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateOrdemProducao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdemProducao_Clientes_ClienteIDCliente",
                table: "OrdemProducao");

            migrationBuilder.DropIndex(
                name: "IX_OrdemProducao_ClienteIDCliente",
                table: "OrdemProducao");

            migrationBuilder.DropColumn(
                name: "ClienteIDCliente",
                table: "OrdemProducao");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "OrdemProducao");

            migrationBuilder.DropColumn(
                name: "IDCliente",
                table: "OrdemProducao");

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "OrdemProducao",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "OrdemProducao",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ClienteIDCliente",
                table: "OrdemProducao",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "OrdemProducao",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "IDCliente",
                table: "OrdemProducao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrdemProducao_ClienteIDCliente",
                table: "OrdemProducao",
                column: "ClienteIDCliente");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdemProducao_Clientes_ClienteIDCliente",
                table: "OrdemProducao",
                column: "ClienteIDCliente",
                principalTable: "Clientes",
                principalColumn: "IDCliente");
        }
    }
}
