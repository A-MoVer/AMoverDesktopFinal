using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatemodelCHecklist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IDModeloMota",
                table: "Checklist",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Obrigatorio",
                table: "Checklist",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Checklist_IDModeloMota",
                table: "Checklist",
                column: "IDModeloMota");

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_ModelosMota_IDModeloMota",
                table: "Checklist",
                column: "IDModeloMota",
                principalTable: "ModelosMota",
                principalColumn: "IDModelo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_ModelosMota_IDModeloMota",
                table: "Checklist");

            migrationBuilder.DropIndex(
                name: "IX_Checklist_IDModeloMota",
                table: "Checklist");

            migrationBuilder.DropColumn(
                name: "IDModeloMota",
                table: "Checklist");

            migrationBuilder.DropColumn(
                name: "Obrigatorio",
                table: "Checklist");
        }
    }
}
