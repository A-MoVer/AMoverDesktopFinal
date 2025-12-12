using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatemodelmotainfopecas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MotasPecasInfo",
                columns: table => new
                {
                    IDMotasPecasInfo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDMota = table.Column<int>(type: "int", nullable: false),
                    IDPeca = table.Column<int>(type: "int", nullable: false),
                    InformacaoAdicional = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotasPecasInfo", x => x.IDMotasPecasInfo);
                    table.ForeignKey(
                        name: "FK_MotasPecasInfo_Motas_IDMota",
                        column: x => x.IDMota,
                        principalTable: "Motas",
                        principalColumn: "IDMota",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MotasPecasInfo_Pecas_IDPeca",
                        column: x => x.IDPeca,
                        principalTable: "Pecas",
                        principalColumn: "IDPeca",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MotasPecasInfo_IDMota",
                table: "MotasPecasInfo",
                column: "IDMota");

            migrationBuilder.CreateIndex(
                name: "IX_MotasPecasInfo_IDPeca",
                table: "MotasPecasInfo",
                column: "IDPeca");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotasPecasInfo");
        }
    }
}
