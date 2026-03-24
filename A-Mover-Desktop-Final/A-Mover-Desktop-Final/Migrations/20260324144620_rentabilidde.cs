using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Migrations
{
    /// <inheritdoc />
    public partial class rentabilidde : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockMotas",
                columns: table => new
                {
                    IDStockMota = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDModelo = table.Column<int>(type: "int", nullable: false),
                    QuantidadeDisponivel = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMotas", x => x.IDStockMota);
                    table.ForeignKey(
                        name: "FK_StockMotas_ModelosMota_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendasMotas",
                columns: table => new
                {
                    IDVendaMota = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDModelo = table.Column<int>(type: "int", nullable: false),
                    IDCliente = table.Column<int>(type: "int", nullable: true),
                    ClienteNome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ClienteEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ClienteTelefone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ClienteNif = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumeroSerie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cor = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Quilometragem = table.Column<double>(type: "float", nullable: false),
                    PrecoVenda = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CustoAquisicao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DespesasManutencao = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataVenda = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendasMotas", x => x.IDVendaMota);
                    table.ForeignKey(
                        name: "FK_VendasMotas_Clientes_IDCliente",
                        column: x => x.IDCliente,
                        principalTable: "Clientes",
                        principalColumn: "IDCliente");
                    table.ForeignKey(
                        name: "FK_VendasMotas_ModelosMota_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockMotas_IDModelo",
                table: "StockMotas",
                column: "IDModelo");

            migrationBuilder.CreateIndex(
                name: "IX_VendasMotas_IDCliente",
                table: "VendasMotas",
                column: "IDCliente");

            migrationBuilder.CreateIndex(
                name: "IX_VendasMotas_IDModelo",
                table: "VendasMotas",
                column: "IDModelo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockMotas");

            migrationBuilder.DropTable(
                name: "VendasMotas");
        }
    }
}
