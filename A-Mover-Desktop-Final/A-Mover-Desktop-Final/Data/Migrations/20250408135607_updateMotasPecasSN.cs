using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateMotasPecasSN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MotasPecasSN_ModeloPecasSN_IDModeloPecaSN",
                table: "MotasPecasSN");

            migrationBuilder.RenameColumn(
                name: "IDModeloPecaSN",
                table: "MotasPecasSN",
                newName: "IDPeca");

            migrationBuilder.RenameIndex(
                name: "IX_MotasPecasSN_IDModeloPecaSN",
                table: "MotasPecasSN",
                newName: "IX_MotasPecasSN_IDPeca");

            migrationBuilder.AddForeignKey(
                name: "FK_MotasPecasSN_Pecas_IDPeca",
                table: "MotasPecasSN",
                column: "IDPeca",
                principalTable: "Pecas",
                principalColumn: "IDPeca",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MotasPecasSN_Pecas_IDPeca",
                table: "MotasPecasSN");

            migrationBuilder.RenameColumn(
                name: "IDPeca",
                table: "MotasPecasSN",
                newName: "IDModeloPecaSN");

            migrationBuilder.RenameIndex(
                name: "IX_MotasPecasSN_IDPeca",
                table: "MotasPecasSN",
                newName: "IX_MotasPecasSN_IDModeloPecaSN");

            migrationBuilder.AddForeignKey(
                name: "FK_MotasPecasSN_ModeloPecasSN_IDModeloPecaSN",
                table: "MotasPecasSN",
                column: "IDModeloPecaSN",
                principalTable: "ModeloPecasSN",
                principalColumn: "IDModeloPSN",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
