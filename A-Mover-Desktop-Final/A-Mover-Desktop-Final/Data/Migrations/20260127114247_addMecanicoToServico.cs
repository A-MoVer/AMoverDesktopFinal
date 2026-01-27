using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class addMecanicoToServico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MecanicoId",
                table: "Servico",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servico_MecanicoId",
                table: "Servico",
                column: "MecanicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servico_Mecanicos_MecanicoId",
                table: "Servico",
                column: "MecanicoId",
                principalTable: "Mecanicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servico_Mecanicos_MecanicoId",
                table: "Servico");

            migrationBuilder.DropIndex(
                name: "IX_Servico_MecanicoId",
                table: "Servico");

            migrationBuilder.DropColumn(
                name: "MecanicoId",
                table: "Servico");
        }
    }
}
