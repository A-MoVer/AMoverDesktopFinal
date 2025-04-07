using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class alteracoesAleatorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClienteIDCliente",
                table: "OrdemProducao",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModeloMotaIDModelo",
                table: "OrdemProducao",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdemProducao_ClienteIDCliente",
                table: "OrdemProducao",
                column: "ClienteIDCliente");

            migrationBuilder.CreateIndex(
                name: "IX_OrdemProducao_ModeloMotaIDModelo",
                table: "OrdemProducao",
                column: "ModeloMotaIDModelo");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdemProducao_Clientes_ClienteIDCliente",
                table: "OrdemProducao",
                column: "ClienteIDCliente",
                principalTable: "Clientes",
                principalColumn: "IDCliente");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdemProducao_ModelosMota_ModeloMotaIDModelo",
                table: "OrdemProducao",
                column: "ModeloMotaIDModelo",
                principalTable: "ModelosMota",
                principalColumn: "IDModelo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdemProducao_Clientes_ClienteIDCliente",
                table: "OrdemProducao");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdemProducao_ModelosMota_ModeloMotaIDModelo",
                table: "OrdemProducao");

            migrationBuilder.DropIndex(
                name: "IX_OrdemProducao_ClienteIDCliente",
                table: "OrdemProducao");

            migrationBuilder.DropIndex(
                name: "IX_OrdemProducao_ModeloMotaIDModelo",
                table: "OrdemProducao");

            migrationBuilder.DropColumn(
                name: "ClienteIDCliente",
                table: "OrdemProducao");

            migrationBuilder.DropColumn(
                name: "ModeloMotaIDModelo",
                table: "OrdemProducao");
        }
    }
}
