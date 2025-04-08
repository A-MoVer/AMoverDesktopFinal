using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatenomeTabela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PecasModelo_ModeloPecasSN_IDModeloPecaSN",
                table: "PecasModelo");

            migrationBuilder.DropForeignKey(
                name: "FK_PecasModelo_Motas_IDMota",
                table: "PecasModelo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PecasModelo",
                table: "PecasModelo");

            migrationBuilder.RenameTable(
                name: "PecasModelo",
                newName: "MotasPecasSN");

            migrationBuilder.RenameIndex(
                name: "IX_PecasModelo_IDMota",
                table: "MotasPecasSN",
                newName: "IX_MotasPecasSN_IDMota");

            migrationBuilder.RenameIndex(
                name: "IX_PecasModelo_IDModeloPecaSN",
                table: "MotasPecasSN",
                newName: "IX_MotasPecasSN_IDModeloPecaSN");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MotasPecasSN",
                table: "MotasPecasSN",
                column: "IDMotasPecasSN");

            migrationBuilder.AddForeignKey(
                name: "FK_MotasPecasSN_ModeloPecasSN_IDModeloPecaSN",
                table: "MotasPecasSN",
                column: "IDModeloPecaSN",
                principalTable: "ModeloPecasSN",
                principalColumn: "IDModeloPSN",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MotasPecasSN_Motas_IDMota",
                table: "MotasPecasSN",
                column: "IDMota",
                principalTable: "Motas",
                principalColumn: "IDMota",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MotasPecasSN_ModeloPecasSN_IDModeloPecaSN",
                table: "MotasPecasSN");

            migrationBuilder.DropForeignKey(
                name: "FK_MotasPecasSN_Motas_IDMota",
                table: "MotasPecasSN");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MotasPecasSN",
                table: "MotasPecasSN");

            migrationBuilder.RenameTable(
                name: "MotasPecasSN",
                newName: "PecasModelo");

            migrationBuilder.RenameIndex(
                name: "IX_MotasPecasSN_IDMota",
                table: "PecasModelo",
                newName: "IX_PecasModelo_IDMota");

            migrationBuilder.RenameIndex(
                name: "IX_MotasPecasSN_IDModeloPecaSN",
                table: "PecasModelo",
                newName: "IX_PecasModelo_IDModeloPecaSN");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PecasModelo",
                table: "PecasModelo",
                column: "IDMotasPecasSN");

            migrationBuilder.AddForeignKey(
                name: "FK_PecasModelo_ModeloPecasSN_IDModeloPecaSN",
                table: "PecasModelo",
                column: "IDModeloPecaSN",
                principalTable: "ModeloPecasSN",
                principalColumn: "IDModeloPSN",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PecasModelo_Motas_IDMota",
                table: "PecasModelo",
                column: "IDMota",
                principalTable: "Motas",
                principalColumn: "IDMota",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
