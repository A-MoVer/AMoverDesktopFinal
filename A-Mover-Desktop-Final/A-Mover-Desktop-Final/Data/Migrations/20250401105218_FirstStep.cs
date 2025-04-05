using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class FirstStep : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checklist",
                columns: table => new
                {
                    IDChecklist = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checklist", x => x.IDChecklist);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IDCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NIF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IDCliente);
                });

            migrationBuilder.CreateTable(
                name: "Documento",
                columns: table => new
                {
                    IDDocumento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documento", x => x.IDDocumento);
                });

            migrationBuilder.CreateTable(
                name: "ModelosMota",
                columns: table => new
                {
                    IDModelo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoProduto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataInicioProducao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataLancamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDescontinuacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelosMota", x => x.IDModelo);
                });

            migrationBuilder.CreateTable(
                name: "Pecas",
                columns: table => new
                {
                    IDPeca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pecas", x => x.IDPeca);
                });

            migrationBuilder.CreateTable(
                name: "DocumentosModelo",
                columns: table => new
                {
                    IDDocumentosModelo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDModelo = table.Column<int>(type: "int", nullable: false),
                    IDDocumento = table.Column<int>(type: "int", nullable: false),
                    Caminho = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosModelo", x => x.IDDocumentosModelo);
                    table.ForeignKey(
                        name: "FK_DocumentosModelo_Documento_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "Documento",
                        principalColumn: "IDDocumento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentosModelo_ModelosMota_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Encomendas",
                columns: table => new
                {
                    IDEncomenda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDModelo = table.Column<int>(type: "int", nullable: false),
                    IDCliente = table.Column<int>(type: "int", nullable: false),
                    DateCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataEntrega = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encomendas", x => x.IDEncomenda);
                    table.ForeignKey(
                        name: "FK_Encomendas_Clientes_IDCliente",
                        column: x => x.IDCliente,
                        principalTable: "Clientes",
                        principalColumn: "IDCliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Encomendas_ModelosMota_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Motas",
                columns: table => new
                {
                    IDMota = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDModelo = table.Column<int>(type: "int", nullable: false),
                    DataRegisto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quilometragem = table.Column<double>(type: "float", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motas", x => x.IDMota);
                    table.ForeignKey(
                        name: "FK_Motas_ModelosMota_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModeloPecasFixas",
                columns: table => new
                {
                    IDMPF = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDModelo = table.Column<int>(type: "int", nullable: false),
                    IDPeca = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeloPecasFixas", x => x.IDMPF);
                    table.ForeignKey(
                        name: "FK_ModeloPecasFixas_ModelosMota_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModeloPecasFixas_Pecas_IDPeca",
                        column: x => x.IDPeca,
                        principalTable: "Pecas",
                        principalColumn: "IDPeca",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModeloPecasSN",
                columns: table => new
                {
                    IDModeloPSN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDModelo = table.Column<int>(type: "int", nullable: false),
                    IDPeca = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeloPecasSN", x => x.IDModeloPSN);
                    table.ForeignKey(
                        name: "FK_ModeloPecasSN_ModelosMota_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModeloPecasSN_Pecas_IDPeca",
                        column: x => x.IDPeca,
                        principalTable: "Pecas",
                        principalColumn: "IDPeca",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdemProducao",
                columns: table => new
                {
                    IDOrdemProducao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDEncomenda = table.Column<int>(type: "int", nullable: false),
                    EncomendaIDEncomenda = table.Column<int>(type: "int", nullable: true),
                    IDCliente = table.Column<int>(type: "int", nullable: false),
                    ClienteIDCliente = table.Column<int>(type: "int", nullable: true),
                    NumeroOrdem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaisDestino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdemProducao", x => x.IDOrdemProducao);
                    table.ForeignKey(
                        name: "FK_OrdemProducao_Clientes_ClienteIDCliente",
                        column: x => x.ClienteIDCliente,
                        principalTable: "Clientes",
                        principalColumn: "IDCliente");
                    table.ForeignKey(
                        name: "FK_OrdemProducao_Encomendas_EncomendaIDEncomenda",
                        column: x => x.EncomendaIDEncomenda,
                        principalTable: "Encomendas",
                        principalColumn: "IDEncomenda");
                });

            migrationBuilder.CreateTable(
                name: "PecasModelo",
                columns: table => new
                {
                    IDMotasPecasSN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDMota = table.Column<int>(type: "int", nullable: false),
                    IDPeca = table.Column<int>(type: "int", nullable: false),
                    NumeroSerie = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PecasModelo", x => x.IDMotasPecasSN);
                    table.ForeignKey(
                        name: "FK_PecasModelo_Motas_IDMota",
                        column: x => x.IDMota,
                        principalTable: "Motas",
                        principalColumn: "IDMota",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PecasModelo_Pecas_IDPeca",
                        column: x => x.IDPeca,
                        principalTable: "Pecas",
                        principalColumn: "IDPeca",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistControlo",
                columns: table => new
                {
                    IDChecklistControlo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDChecklist = table.Column<int>(type: "int", nullable: false),
                    IDOrdemProducao = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistControlo", x => x.IDChecklistControlo);
                    table.ForeignKey(
                        name: "FK_ChecklistControlo_Checklist_IDChecklist",
                        column: x => x.IDChecklist,
                        principalTable: "Checklist",
                        principalColumn: "IDChecklist",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChecklistControlo_OrdemProducao_IDOrdemProducao",
                        column: x => x.IDOrdemProducao,
                        principalTable: "OrdemProducao",
                        principalColumn: "IDOrdemProducao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistEmbalagem",
                columns: table => new
                {
                    IDChecklistEmbalagem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDChecklist = table.Column<int>(type: "int", nullable: false),
                    IDOrdemProducao = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistEmbalagem", x => x.IDChecklistEmbalagem);
                    table.ForeignKey(
                        name: "FK_ChecklistEmbalagem_Checklist_IDChecklist",
                        column: x => x.IDChecklist,
                        principalTable: "Checklist",
                        principalColumn: "IDChecklist",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChecklistEmbalagem_OrdemProducao_IDOrdemProducao",
                        column: x => x.IDOrdemProducao,
                        principalTable: "OrdemProducao",
                        principalColumn: "IDOrdemProducao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistMontagem",
                columns: table => new
                {
                    IDChecklistMontagem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDChecklist = table.Column<int>(type: "int", nullable: false),
                    IDOrdemProducao = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistMontagem", x => x.IDChecklistMontagem);
                    table.ForeignKey(
                        name: "FK_ChecklistMontagem_Checklist_IDChecklist",
                        column: x => x.IDChecklist,
                        principalTable: "Checklist",
                        principalColumn: "IDChecklist",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChecklistMontagem_OrdemProducao_IDOrdemProducao",
                        column: x => x.IDOrdemProducao,
                        principalTable: "OrdemProducao",
                        principalColumn: "IDOrdemProducao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistControlo_IDChecklist",
                table: "ChecklistControlo",
                column: "IDChecklist");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistControlo_IDOrdemProducao",
                table: "ChecklistControlo",
                column: "IDOrdemProducao");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistEmbalagem_IDChecklist",
                table: "ChecklistEmbalagem",
                column: "IDChecklist");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistEmbalagem_IDOrdemProducao",
                table: "ChecklistEmbalagem",
                column: "IDOrdemProducao");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistMontagem_IDChecklist",
                table: "ChecklistMontagem",
                column: "IDChecklist");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistMontagem_IDOrdemProducao",
                table: "ChecklistMontagem",
                column: "IDOrdemProducao");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosModelo_IDModelo",
                table: "DocumentosModelo",
                column: "IDModelo");

            migrationBuilder.CreateIndex(
                name: "IX_Encomendas_IDCliente",
                table: "Encomendas",
                column: "IDCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Encomendas_IDModelo",
                table: "Encomendas",
                column: "IDModelo");

            migrationBuilder.CreateIndex(
                name: "IX_ModeloPecasFixas_IDModelo",
                table: "ModeloPecasFixas",
                column: "IDModelo");

            migrationBuilder.CreateIndex(
                name: "IX_ModeloPecasFixas_IDPeca",
                table: "ModeloPecasFixas",
                column: "IDPeca");

            migrationBuilder.CreateIndex(
                name: "IX_ModeloPecasSN_IDModelo",
                table: "ModeloPecasSN",
                column: "IDModelo");

            migrationBuilder.CreateIndex(
                name: "IX_ModeloPecasSN_IDPeca",
                table: "ModeloPecasSN",
                column: "IDPeca");

            migrationBuilder.CreateIndex(
                name: "IX_Motas_IDModelo",
                table: "Motas",
                column: "IDModelo");

            migrationBuilder.CreateIndex(
                name: "IX_OrdemProducao_ClienteIDCliente",
                table: "OrdemProducao",
                column: "ClienteIDCliente");

            migrationBuilder.CreateIndex(
                name: "IX_OrdemProducao_EncomendaIDEncomenda",
                table: "OrdemProducao",
                column: "EncomendaIDEncomenda");

            migrationBuilder.CreateIndex(
                name: "IX_PecasModelo_IDMota",
                table: "PecasModelo",
                column: "IDMota");

            migrationBuilder.CreateIndex(
                name: "IX_PecasModelo_IDPeca",
                table: "PecasModelo",
                column: "IDPeca");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChecklistControlo");

            migrationBuilder.DropTable(
                name: "ChecklistEmbalagem");

            migrationBuilder.DropTable(
                name: "ChecklistMontagem");

            migrationBuilder.DropTable(
                name: "DocumentosModelo");

            migrationBuilder.DropTable(
                name: "ModeloPecasFixas");

            migrationBuilder.DropTable(
                name: "ModeloPecasSN");

            migrationBuilder.DropTable(
                name: "PecasModelo");

            migrationBuilder.DropTable(
                name: "Checklist");

            migrationBuilder.DropTable(
                name: "OrdemProducao");

            migrationBuilder.DropTable(
                name: "Documento");

            migrationBuilder.DropTable(
                name: "Motas");

            migrationBuilder.DropTable(
                name: "Pecas");

            migrationBuilder.DropTable(
                name: "Encomendas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "ModelosMota");
        }
    }
}
