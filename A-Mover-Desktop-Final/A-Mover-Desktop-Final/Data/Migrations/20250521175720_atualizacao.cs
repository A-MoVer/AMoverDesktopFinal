using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class atualizacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EncomendaIDEncomenda",
                table: "OrdemProducao",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdemProducao_EncomendaIDEncomenda",
                table: "OrdemProducao",
                column: "EncomendaIDEncomenda");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdemProducao_Encomendas_EncomendaIDEncomenda",
                table: "OrdemProducao",
                column: "EncomendaIDEncomenda",
                principalTable: "Encomendas",
                principalColumn: "IDEncomenda");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdemProducao_Encomendas_EncomendaIDEncomenda",
                table: "OrdemProducao");

            migrationBuilder.DropIndex(
                name: "IX_OrdemProducao_EncomendaIDEncomenda",
                table: "OrdemProducao");

            migrationBuilder.DropColumn(
                name: "EncomendaIDEncomenda",
                table: "OrdemProducao");
        }
    }
}
