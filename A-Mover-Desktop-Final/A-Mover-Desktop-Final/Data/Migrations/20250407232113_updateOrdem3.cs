using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateOrdem3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModeloPecasSN_Pecas_IDPeca",
                table: "ModeloPecasSN");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdemProducao_Encomendas_EncomendaIDEncomenda",
                table: "OrdemProducao");

            migrationBuilder.DropIndex(
                name: "IX_OrdemProducao_EncomendaIDEncomenda",
                table: "OrdemProducao");

            migrationBuilder.DropIndex(
                name: "IX_Motas_IDOrdemProducao",
                table: "Motas");

            migrationBuilder.DropColumn(
                name: "EncomendaIDEncomenda",
                table: "OrdemProducao");

            migrationBuilder.CreateIndex(
                name: "IX_OrdemProducao_IDEncomenda",
                table: "OrdemProducao",
                column: "IDEncomenda");

            migrationBuilder.CreateIndex(
                name: "IX_Motas_IDOrdemProducao",
                table: "Motas",
                column: "IDOrdemProducao",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ModeloPecasSN_Pecas_IDPeca",
                table: "ModeloPecasSN",
                column: "IDPeca",
                principalTable: "Pecas",
                principalColumn: "IDPeca",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdemProducao_Encomendas_IDEncomenda",
                table: "OrdemProducao",
                column: "IDEncomenda",
                principalTable: "Encomendas",
                principalColumn: "IDEncomenda",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModeloPecasSN_Pecas_IDPeca",
                table: "ModeloPecasSN");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdemProducao_Encomendas_IDEncomenda",
                table: "OrdemProducao");

            migrationBuilder.DropIndex(
                name: "IX_OrdemProducao_IDEncomenda",
                table: "OrdemProducao");

            migrationBuilder.DropIndex(
                name: "IX_Motas_IDOrdemProducao",
                table: "Motas");

            migrationBuilder.AddColumn<int>(
                name: "EncomendaIDEncomenda",
                table: "OrdemProducao",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdemProducao_EncomendaIDEncomenda",
                table: "OrdemProducao",
                column: "EncomendaIDEncomenda");

            migrationBuilder.CreateIndex(
                name: "IX_Motas_IDOrdemProducao",
                table: "Motas",
                column: "IDOrdemProducao");

            migrationBuilder.AddForeignKey(
                name: "FK_ModeloPecasSN_Pecas_IDPeca",
                table: "ModeloPecasSN",
                column: "IDPeca",
                principalTable: "Pecas",
                principalColumn: "IDPeca",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdemProducao_Encomendas_EncomendaIDEncomenda",
                table: "OrdemProducao",
                column: "EncomendaIDEncomenda",
                principalTable: "Encomendas",
                principalColumn: "IDEncomenda");
        }
    }
}
