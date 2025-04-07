using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForeignKeyConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModeloPecasSN_ModelosMota_IDModelo",
                table: "ModeloPecasSN");

            migrationBuilder.DropForeignKey(
                name: "FK_ModeloPecasSN_Pecas_IDPeca",
                table: "ModeloPecasSN");

            migrationBuilder.DropForeignKey(
                name: "FK_PecasModelo_Pecas_IDPeca",
                table: "PecasModelo");

            migrationBuilder.RenameColumn(
                name: "IDPeca",
                table: "PecasModelo",
                newName: "IDModeloPecaSN");

            migrationBuilder.RenameIndex(
                name: "IX_PecasModelo_IDPeca",
                table: "PecasModelo",
                newName: "IX_PecasModelo_IDModeloPecaSN");

            migrationBuilder.AddColumn<string>(
                name: "NumeroIdentificacao",
                table: "Motas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ModeloMotaIDModelo",
                table: "ModeloPecasSN",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModeloPecasSN_ModeloMotaIDModelo",
                table: "ModeloPecasSN",
                column: "ModeloMotaIDModelo");

            migrationBuilder.AddForeignKey(
                name: "FK_ModeloPecasSN_ModelosMota_IDModelo",
                table: "ModeloPecasSN",
                column: "IDModelo",
                principalTable: "ModelosMota",
                principalColumn: "IDModelo",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModeloPecasSN_ModelosMota_ModeloMotaIDModelo",
                table: "ModeloPecasSN",
                column: "ModeloMotaIDModelo",
                principalTable: "ModelosMota",
                principalColumn: "IDModelo");

            migrationBuilder.AddForeignKey(
                name: "FK_ModeloPecasSN_Pecas_IDPeca",
                table: "ModeloPecasSN",
                column: "IDPeca",
                principalTable: "Pecas",
                principalColumn: "IDPeca",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PecasModelo_ModeloPecasSN_IDModeloPecaSN",
                table: "PecasModelo",
                column: "IDModeloPecaSN",
                principalTable: "ModeloPecasSN",
                principalColumn: "IDModeloPSN",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModeloPecasSN_ModelosMota_IDModelo",
                table: "ModeloPecasSN");

            migrationBuilder.DropForeignKey(
                name: "FK_ModeloPecasSN_ModelosMota_ModeloMotaIDModelo",
                table: "ModeloPecasSN");

            migrationBuilder.DropForeignKey(
                name: "FK_ModeloPecasSN_Pecas_IDPeca",
                table: "ModeloPecasSN");

            migrationBuilder.DropForeignKey(
                name: "FK_PecasModelo_ModeloPecasSN_IDModeloPecaSN",
                table: "PecasModelo");

            migrationBuilder.DropIndex(
                name: "IX_ModeloPecasSN_ModeloMotaIDModelo",
                table: "ModeloPecasSN");

            migrationBuilder.DropColumn(
                name: "NumeroIdentificacao",
                table: "Motas");

            migrationBuilder.DropColumn(
                name: "ModeloMotaIDModelo",
                table: "ModeloPecasSN");

            migrationBuilder.RenameColumn(
                name: "IDModeloPecaSN",
                table: "PecasModelo",
                newName: "IDPeca");

            migrationBuilder.RenameIndex(
                name: "IX_PecasModelo_IDModeloPecaSN",
                table: "PecasModelo",
                newName: "IX_PecasModelo_IDPeca");

            migrationBuilder.AddForeignKey(
                name: "FK_ModeloPecasSN_ModelosMota_IDModelo",
                table: "ModeloPecasSN",
                column: "IDModelo",
                principalTable: "ModelosMota",
                principalColumn: "IDModelo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModeloPecasSN_Pecas_IDPeca",
                table: "ModeloPecasSN",
                column: "IDPeca",
                principalTable: "Pecas",
                principalColumn: "IDPeca",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PecasModelo_Pecas_IDPeca",
                table: "PecasModelo",
                column: "IDPeca",
                principalTable: "Pecas",
                principalColumn: "IDPeca",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
