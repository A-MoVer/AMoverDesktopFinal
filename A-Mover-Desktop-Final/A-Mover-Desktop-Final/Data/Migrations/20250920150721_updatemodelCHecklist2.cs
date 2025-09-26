using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatemodelCHecklist2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_ModelosMota_IDModeloMota",
                table: "Checklist");

            migrationBuilder.DropColumn(
                name: "Obrigatorio",
                table: "Checklist");

            migrationBuilder.RenameColumn(
                name: "IDModeloMota",
                table: "Checklist",
                newName: "IDModelo");

            migrationBuilder.RenameIndex(
                name: "IX_Checklist_IDModeloMota",
                table: "Checklist",
                newName: "IX_Checklist_IDModelo");

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_ModelosMota_IDModelo",
                table: "Checklist",
                column: "IDModelo",
                principalTable: "ModelosMota",
                principalColumn: "IDModelo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_ModelosMota_IDModelo",
                table: "Checklist");

            migrationBuilder.RenameColumn(
                name: "IDModelo",
                table: "Checklist",
                newName: "IDModeloMota");

            migrationBuilder.RenameIndex(
                name: "IX_Checklist_IDModelo",
                table: "Checklist",
                newName: "IX_Checklist_IDModeloMota");

            migrationBuilder.AddColumn<bool>(
                name: "Obrigatorio",
                table: "Checklist",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_ModelosMota_IDModeloMota",
                table: "Checklist",
                column: "IDModeloMota",
                principalTable: "ModelosMota",
                principalColumn: "IDModelo");
        }
    }
}
