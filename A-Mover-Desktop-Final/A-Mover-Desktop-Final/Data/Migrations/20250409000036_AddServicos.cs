using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServicos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Servico",
                columns: table => new
                {
                    IDServico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDMota = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    DataServico = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servico", x => x.IDServico);
                    table.ForeignKey(
                        name: "FK_Servico_Motas_IDMota",
                        column: x => x.IDMota,
                        principalTable: "Motas",
                        principalColumn: "IDMota",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServicosPecasAlteradas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDServico = table.Column<int>(type: "int", nullable: false),
                    IDMotasPecasSN = table.Column<int>(type: "int", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicosPecasAlteradas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ServicosPecasAlteradas_MotasPecasSN_IDMotasPecasSN",
                        column: x => x.IDMotasPecasSN,
                        principalTable: "MotasPecasSN",
                        principalColumn: "IDMotasPecasSN",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServicosPecasAlteradas_Servico_IDServico",
                        column: x => x.IDServico,
                        principalTable: "Servico",
                        principalColumn: "IDServico",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Servico_IDMota",
                table: "Servico",
                column: "IDMota");

            migrationBuilder.CreateIndex(
                name: "IX_ServicosPecasAlteradas_IDMotasPecasSN",
                table: "ServicosPecasAlteradas",
                column: "IDMotasPecasSN");

            migrationBuilder.CreateIndex(
                name: "IX_ServicosPecasAlteradas_IDServico",
                table: "ServicosPecasAlteradas",
                column: "IDServico");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServicosPecasAlteradas");

            migrationBuilder.DropTable(
                name: "Servico");
        }
    }
}
