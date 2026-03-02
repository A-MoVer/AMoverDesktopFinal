using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Migrations
{
    /// <inheritdoc />
    public partial class InitialProductionFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Checklist",
                columns: table => new
                {
                    IDChecklist = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false)
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
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataModificacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UltimaEncomenda = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "Fornecedores",
                columns: table => new
                {
                    IDFornecedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.IDFornecedor);
                });

            migrationBuilder.CreateTable(
                name: "Mecanicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Telemovel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OficinaId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MustChangePassword = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mecanicos", x => x.Id);
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
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelosMota", x => x.IDModelo);
                });

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    IdUtilizador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.IdUtilizador);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pecas",
                columns: table => new
                {
                    IDPeca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FornecedorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pecas", x => x.IDPeca);
                    table.ForeignKey(
                        name: "FK_Pecas_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "IDFornecedor",
                        onDelete: ReferentialAction.Restrict);
                });

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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChecklistModelo_ModelosMota_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentosModelo_ModelosMota_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Encomendas",
                columns: table => new
                {
                    IDEncomenda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDModelo = table.Column<int>(type: "int", nullable: false),
                    IDCliente = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Encomendas_ModelosMota_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    IDCompra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PecaId = table.Column<int>(type: "int", nullable: false),
                    DataPedido = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FornecedorId = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.IDCompra);
                    table.ForeignKey(
                        name: "FK_Compras_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "IDFornecedor",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compras_Pecas_PecaId",
                        column: x => x.PecaId,
                        principalTable: "Pecas",
                        principalColumn: "IDPeca",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModeloPecasFixas",
                columns: table => new
                {
                    IDMPF = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDModelo = table.Column<int>(type: "int", nullable: false),
                    IDPeca = table.Column<int>(type: "int", nullable: false),
                    EspecificacaoPadrao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeloPecasFixas", x => x.IDMPF);
                    table.ForeignKey(
                        name: "FK_ModeloPecasFixas_ModelosMota_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModeloPecasFixas_Pecas_IDPeca",
                        column: x => x.IDPeca,
                        principalTable: "Pecas",
                        principalColumn: "IDPeca",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModeloPecasSN",
                columns: table => new
                {
                    IDModeloPSN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDModelo = table.Column<int>(type: "int", nullable: false),
                    IDPeca = table.Column<int>(type: "int", nullable: false),
                    EspecificacaoPadrao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModeloMotaIDModelo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeloPecasSN", x => x.IDModeloPSN);
                    table.ForeignKey(
                        name: "FK_ModeloPecasSN_ModelosMota_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModeloPecasSN_ModelosMota_ModeloMotaIDModelo",
                        column: x => x.ModeloMotaIDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo");
                    table.ForeignKey(
                        name: "FK_ModeloPecasSN_Pecas_IDPeca",
                        column: x => x.IDPeca,
                        principalTable: "Pecas",
                        principalColumn: "IDPeca",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrdemProducao",
                columns: table => new
                {
                    IDOrdemProducao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDEncomenda = table.Column<int>(type: "int", nullable: false),
                    NumeroOrdem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    PaisDestino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClienteIDCliente = table.Column<int>(type: "int", nullable: true),
                    EncomendaIDEncomenda = table.Column<int>(type: "int", nullable: true),
                    ModeloMotaIDModelo = table.Column<int>(type: "int", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_OrdemProducao_Encomendas_IDEncomenda",
                        column: x => x.IDEncomenda,
                        principalTable: "Encomendas",
                        principalColumn: "IDEncomenda",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdemProducao_ModelosMota_ModeloMotaIDModelo",
                        column: x => x.ModeloMotaIDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo");
                });

            migrationBuilder.CreateTable(
                name: "MateriaisRecebidos",
                columns: table => new
                {
                    IDMaterialRecebido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PecaId = table.Column<int>(type: "int", nullable: false),
                    FornecedorId = table.Column<int>(type: "int", nullable: false),
                    DataRececao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Lote = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Documento = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CompraId = table.Column<int>(type: "int", nullable: true),
                    NumeroChassiPrimeiraPeca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NumeroChassiUltimaPeca = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriaisRecebidos", x => x.IDMaterialRecebido);
                    table.ForeignKey(
                        name: "FK_MateriaisRecebidos_Compras_CompraId",
                        column: x => x.CompraId,
                        principalTable: "Compras",
                        principalColumn: "IDCompra");
                    table.ForeignKey(
                        name: "FK_MateriaisRecebidos_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "IDFornecedor",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MateriaisRecebidos_Pecas_PecaId",
                        column: x => x.PecaId,
                        principalTable: "Pecas",
                        principalColumn: "IDPeca",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistControlo",
                columns: table => new
                {
                    IDChecklistControlo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDChecklist = table.Column<int>(type: "int", nullable: false),
                    IDOrdemProducao = table.Column<int>(type: "int", nullable: false),
                    ControloFinal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistControlo", x => x.IDChecklistControlo);
                    table.ForeignKey(
                        name: "FK_ChecklistControlo_Checklist_IDChecklist",
                        column: x => x.IDChecklist,
                        principalTable: "Checklist",
                        principalColumn: "IDChecklist",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChecklistControlo_OrdemProducao_IDOrdemProducao",
                        column: x => x.IDOrdemProducao,
                        principalTable: "OrdemProducao",
                        principalColumn: "IDOrdemProducao",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistEmbalagem",
                columns: table => new
                {
                    IDChecklistEmbalagem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDChecklist = table.Column<int>(type: "int", nullable: false),
                    IDOrdemProducao = table.Column<int>(type: "int", nullable: false),
                    Incluido = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistEmbalagem", x => x.IDChecklistEmbalagem);
                    table.ForeignKey(
                        name: "FK_ChecklistEmbalagem_Checklist_IDChecklist",
                        column: x => x.IDChecklist,
                        principalTable: "Checklist",
                        principalColumn: "IDChecklist",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChecklistEmbalagem_OrdemProducao_IDOrdemProducao",
                        column: x => x.IDOrdemProducao,
                        principalTable: "OrdemProducao",
                        principalColumn: "IDOrdemProducao",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistMontagem",
                columns: table => new
                {
                    IDChecklistMontagem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDChecklist = table.Column<int>(type: "int", nullable: false),
                    IDOrdemProducao = table.Column<int>(type: "int", nullable: false),
                    Verificado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistMontagem", x => x.IDChecklistMontagem);
                    table.ForeignKey(
                        name: "FK_ChecklistMontagem_Checklist_IDChecklist",
                        column: x => x.IDChecklist,
                        principalTable: "Checklist",
                        principalColumn: "IDChecklist",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChecklistMontagem_OrdemProducao_IDOrdemProducao",
                        column: x => x.IDOrdemProducao,
                        principalTable: "OrdemProducao",
                        principalColumn: "IDOrdemProducao",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Motas",
                columns: table => new
                {
                    IDMota = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDModelo = table.Column<int>(type: "int", nullable: false),
                    IDOrdemProducao = table.Column<int>(type: "int", nullable: false),
                    NumeroIdentificacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataRegisto = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quilometragem = table.Column<double>(type: "float", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    QrToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrCriadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motas", x => x.IDMota);
                    table.ForeignKey(
                        name: "FK_Motas_ModelosMota_IDModelo",
                        column: x => x.IDModelo,
                        principalTable: "ModelosMota",
                        principalColumn: "IDModelo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Motas_OrdemProducao_IDOrdemProducao",
                        column: x => x.IDOrdemProducao,
                        principalTable: "OrdemProducao",
                        principalColumn: "IDOrdemProducao",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MotasPecasInfo",
                columns: table => new
                {
                    IDMotasPecasInfo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDMota = table.Column<int>(type: "int", nullable: false),
                    IDPeca = table.Column<int>(type: "int", nullable: false),
                    InformacaoAdicional = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotasPecasInfo", x => x.IDMotasPecasInfo);
                    table.ForeignKey(
                        name: "FK_MotasPecasInfo_Motas_IDMota",
                        column: x => x.IDMota,
                        principalTable: "Motas",
                        principalColumn: "IDMota",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MotasPecasInfo_Pecas_IDPeca",
                        column: x => x.IDPeca,
                        principalTable: "Pecas",
                        principalColumn: "IDPeca",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MotasPecasSN",
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
                    table.PrimaryKey("PK_MotasPecasSN", x => x.IDMotasPecasSN);
                    table.ForeignKey(
                        name: "FK_MotasPecasSN_Motas_IDMota",
                        column: x => x.IDMota,
                        principalTable: "Motas",
                        principalColumn: "IDMota",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MotasPecasSN_Pecas_IDPeca",
                        column: x => x.IDPeca,
                        principalTable: "Pecas",
                        principalColumn: "IDPeca",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    DataConclusao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NotasServico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MecanicoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servico", x => x.IDServico);
                    table.ForeignKey(
                        name: "FK_Servico_Mecanicos_MecanicoId",
                        column: x => x.MecanicoId,
                        principalTable: "Mecanicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Servico_Motas_IDMota",
                        column: x => x.IDMota,
                        principalTable: "Motas",
                        principalColumn: "IDMota",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UtilizadorMota",
                columns: table => new
                {
                    IDUtilizadorMota = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDMota = table.Column<int>(type: "int", nullable: false),
                    IdUtilizador = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    DataInativacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotivoInativacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilizadorMota", x => x.IDUtilizadorMota);
                    table.ForeignKey(
                        name: "FK_UtilizadorMota_Motas_IDMota",
                        column: x => x.IDMota,
                        principalTable: "Motas",
                        principalColumn: "IDMota",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UtilizadorMota_Utilizadores_IdUtilizador",
                        column: x => x.IdUtilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "IdUtilizador",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

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
                name: "IX_ChecklistModelo_IDChecklist",
                table: "ChecklistModelo",
                column: "IDChecklist");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistModelo_IDModelo",
                table: "ChecklistModelo",
                column: "IDModelo");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistMontagem_IDChecklist",
                table: "ChecklistMontagem",
                column: "IDChecklist");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistMontagem_IDOrdemProducao",
                table: "ChecklistMontagem",
                column: "IDOrdemProducao");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_FornecedorId",
                table: "Compras",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_PecaId",
                table: "Compras",
                column: "PecaId");

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
                name: "IX_MateriaisRecebidos_CompraId",
                table: "MateriaisRecebidos",
                column: "CompraId");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaisRecebidos_FornecedorId",
                table: "MateriaisRecebidos",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaisRecebidos_PecaId",
                table: "MateriaisRecebidos",
                column: "PecaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mecanicos_OficinaId_Email",
                table: "Mecanicos",
                columns: new[] { "OficinaId", "Email" },
                unique: true);

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
                name: "IX_ModeloPecasSN_ModeloMotaIDModelo",
                table: "ModeloPecasSN",
                column: "ModeloMotaIDModelo");

            migrationBuilder.CreateIndex(
                name: "IX_Motas_IDModelo",
                table: "Motas",
                column: "IDModelo");

            migrationBuilder.CreateIndex(
                name: "IX_Motas_IDOrdemProducao",
                table: "Motas",
                column: "IDOrdemProducao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MotasPecasInfo_IDMota",
                table: "MotasPecasInfo",
                column: "IDMota");

            migrationBuilder.CreateIndex(
                name: "IX_MotasPecasInfo_IDPeca",
                table: "MotasPecasInfo",
                column: "IDPeca");

            migrationBuilder.CreateIndex(
                name: "IX_MotasPecasSN_IDMota",
                table: "MotasPecasSN",
                column: "IDMota");

            migrationBuilder.CreateIndex(
                name: "IX_MotasPecasSN_IDPeca",
                table: "MotasPecasSN",
                column: "IDPeca");

            migrationBuilder.CreateIndex(
                name: "IX_OrdemProducao_ClienteIDCliente",
                table: "OrdemProducao",
                column: "ClienteIDCliente");

            migrationBuilder.CreateIndex(
                name: "IX_OrdemProducao_EncomendaIDEncomenda",
                table: "OrdemProducao",
                column: "EncomendaIDEncomenda");

            migrationBuilder.CreateIndex(
                name: "IX_OrdemProducao_IDEncomenda",
                table: "OrdemProducao",
                column: "IDEncomenda");

            migrationBuilder.CreateIndex(
                name: "IX_OrdemProducao_ModeloMotaIDModelo",
                table: "OrdemProducao",
                column: "ModeloMotaIDModelo");

            migrationBuilder.CreateIndex(
                name: "IX_Pecas_FornecedorId",
                table: "Pecas",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Servico_IDMota",
                table: "Servico",
                column: "IDMota");

            migrationBuilder.CreateIndex(
                name: "IX_Servico_MecanicoId",
                table: "Servico",
                column: "MecanicoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicosPecasAlteradas_IDMotasPecasSN",
                table: "ServicosPecasAlteradas",
                column: "IDMotasPecasSN");

            migrationBuilder.CreateIndex(
                name: "IX_ServicosPecasAlteradas_IDServico",
                table: "ServicosPecasAlteradas",
                column: "IDServico");

            migrationBuilder.CreateIndex(
                name: "IX_UtilizadorMota_IDMota",
                table: "UtilizadorMota",
                column: "IDMota");

            migrationBuilder.CreateIndex(
                name: "IX_UtilizadorMota_IdUtilizador",
                table: "UtilizadorMota",
                column: "IdUtilizador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ChecklistControlo");

            migrationBuilder.DropTable(
                name: "ChecklistEmbalagem");

            migrationBuilder.DropTable(
                name: "ChecklistModelo");

            migrationBuilder.DropTable(
                name: "ChecklistMontagem");

            migrationBuilder.DropTable(
                name: "DocumentosModelo");

            migrationBuilder.DropTable(
                name: "MateriaisRecebidos");

            migrationBuilder.DropTable(
                name: "ModeloPecasFixas");

            migrationBuilder.DropTable(
                name: "ModeloPecasSN");

            migrationBuilder.DropTable(
                name: "MotasPecasInfo");

            migrationBuilder.DropTable(
                name: "ServicosPecasAlteradas");

            migrationBuilder.DropTable(
                name: "UtilizadorMota");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Checklist");

            migrationBuilder.DropTable(
                name: "Documento");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "MotasPecasSN");

            migrationBuilder.DropTable(
                name: "Servico");

            migrationBuilder.DropTable(
                name: "Utilizadores");

            migrationBuilder.DropTable(
                name: "Pecas");

            migrationBuilder.DropTable(
                name: "Mecanicos");

            migrationBuilder.DropTable(
                name: "Motas");

            migrationBuilder.DropTable(
                name: "Fornecedores");

            migrationBuilder.DropTable(
                name: "OrdemProducao");

            migrationBuilder.DropTable(
                name: "Encomendas");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "ModelosMota");
        }
    }
}
