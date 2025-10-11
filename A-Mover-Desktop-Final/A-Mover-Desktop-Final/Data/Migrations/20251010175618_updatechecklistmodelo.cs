using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatechecklistmodelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_ModelosMota_IDModelo",
                table: "Checklist");

            migrationBuilder.DropIndex(
                name: "IX_Checklist_IDModelo",
                table: "Checklist");

            migrationBuilder.DropColumn(
                name: "IDModelo",
                table: "Checklist");

            migrationBuilder.CreateTable(
                name: "ChecklistModelo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDChecklist = table.Column<int>(type: "int", nullable: false),
                    IDModelo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistModelo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChecklistModelo_Checklist_IDChecklist",
                        column: x => x.IDChecklist,
                        principalTable: "Checklist",
                        principalColumn: "IDChecklist",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChecklistModelo_ModelosMota_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistModelo_IDChecklist",
                table: "ChecklistModelo",
                column: "IDChecklist");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistModelo_IDModelo",
                table: "ChecklistModelo",
                column: "IDModelo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecklistModelo");

            migrationBuilder.AddColumn<int>(
                name: "IDModelo",
                table: "Checklist",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Checklist_IDModelo",
                table: "Checklist",
                column: "IDModelo");

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_ModelosMota_IDModelo",
                table: "Checklist",
                column: "IDModelo",
                principalTable: "ModelosMota",
                principalColumn: "IDModelo");
        }
    }
}
