using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Migrations
{
    /// <inheritdoc />
    public partial class vendaMotas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Motas_OrdemProducao_IDOrdemProducao",
                table: "Motas");

            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_StockMotas_IDModelo' AND object_id = OBJECT_ID('StockMotas'))
    DROP INDEX [IX_StockMotas_IDModelo] ON [StockMotas];
");

            migrationBuilder.DropIndex(
                name: "IX_Motas_IDOrdemProducao",
                table: "Motas");

            migrationBuilder.AlterColumn<int>(
                name: "IDOrdemProducao",
                table: "Motas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IDOrdemProducao",
                table: "ChecklistControlo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.Sql(@"
IF OBJECT_ID('StockMotas') IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_StockMotas_IDModelo' AND object_id = OBJECT_ID('StockMotas'))
        CREATE INDEX [IX_StockMotas_IDModelo] ON [StockMotas]([IDModelo]);
END
");

            migrationBuilder.CreateIndex(
                name: "IX_Motas_IDOrdemProducao",
                table: "Motas",
                column: "IDOrdemProducao",
                unique: true,
                filter: "[IDOrdemProducao] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Motas_OrdemProducao_IDOrdemProducao",
                table: "Motas",
                column: "IDOrdemProducao",
                principalTable: "OrdemProducao",
                principalColumn: "IDOrdemProducao");

            migrationBuilder.Sql(@"
IF OBJECT_ID('StockMotas') IS NOT NULL
    ALTER TABLE [StockMotas] WITH CHECK ADD CONSTRAINT [FK_StockMotas_ModelosMota_IDModelo]
    FOREIGN KEY([IDModelo]) REFERENCES [ModelosMota] ([IDModelo]) ON DELETE CASCADE;
");

            migrationBuilder.Sql(@"
IF OBJECT_ID('VendasMotas') IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_VendasMotas_Clientes_IDCliente')
        ALTER TABLE [VendasMotas] WITH CHECK ADD CONSTRAINT [FK_VendasMotas_Clientes_IDCliente]
        FOREIGN KEY([IDCliente]) REFERENCES [Clientes] ([IDCliente]);

    IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_VendasMotas_ModelosMota_IDModelo')
        ALTER TABLE [VendasMotas] WITH CHECK ADD CONSTRAINT [FK_VendasMotas_ModelosMota_IDModelo]
        FOREIGN KEY([IDModelo]) REFERENCES [ModelosMota] ([IDModelo]) ON DELETE CASCADE;
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Motas_OrdemProducao_IDOrdemProducao",
                table: "Motas");

            migrationBuilder.Sql(@"
IF OBJECT_ID('StockMotas') IS NOT NULL AND EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_StockMotas_ModelosMota_IDModelo')
    ALTER TABLE [StockMotas] DROP CONSTRAINT [FK_StockMotas_ModelosMota_IDModelo];
");

            migrationBuilder.Sql(@"
IF OBJECT_ID('VendasMotas') IS NOT NULL AND EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_VendasMotas_Clientes_IDCliente')
    ALTER TABLE [VendasMotas] DROP CONSTRAINT [FK_VendasMotas_Clientes_IDCliente];
IF OBJECT_ID('VendasMotas') IS NOT NULL AND EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_VendasMotas_ModelosMota_IDModelo')
    ALTER TABLE [VendasMotas] DROP CONSTRAINT [FK_VendasMotas_ModelosMota_IDModelo];
");

            migrationBuilder.Sql(@"
IF OBJECT_ID('StockMotas') IS NOT NULL AND EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_StockMotas_IDModelo' AND object_id = OBJECT_ID('StockMotas'))
    DROP INDEX [IX_StockMotas_IDModelo] ON [StockMotas];
");

            migrationBuilder.DropIndex(
                name: "IX_Motas_IDOrdemProducao",
                table: "Motas");

            migrationBuilder.AlterColumn<int>(
                name: "IDOrdemProducao",
                table: "Motas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IDOrdemProducao",
                table: "ChecklistControlo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_StockMotas_IDModelo",
                table: "StockMotas",
                column: "IDModelo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Motas_IDOrdemProducao",
                table: "Motas",
                column: "IDOrdemProducao",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Motas_OrdemProducao_IDOrdemProducao",
                table: "Motas",
                column: "IDOrdemProducao",
                principalTable: "OrdemProducao",
                principalColumn: "IDOrdemProducao",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
