using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateMotas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IDOrdemProducao",
                table: "Motas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Motas_IDOrdemProducao",
                table: "Motas",
                column: "IDOrdemProducao");

            migrationBuilder.AddForeignKey(
                name: "FK_Motas_OrdemProducao_IDOrdemProducao",
                table: "Motas",
                column: "IDOrdemProducao",
                principalTable: "OrdemProducao",
                principalColumn: "IDOrdemProducao",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Motas_OrdemProducao_IDOrdemProducao",
                table: "Motas");

            migrationBuilder.DropIndex(
                name: "IX_Motas_IDOrdemProducao",
                table: "Motas");

            migrationBuilder.DropColumn(
                name: "IDOrdemProducao",
                table: "Motas");
        }
    }
}
