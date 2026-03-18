IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END

IF OBJECT_ID(N'[StockMotas]') IS NULL
BEGIN
    CREATE TABLE [StockMotas] (
        [IDStockMota] int NOT NULL IDENTITY(1, 1),
        [IDModelo] int NOT NULL,
        [QuantidadeDisponivel] int NOT NULL,
        [DataCriacao] datetime2 NOT NULL,
        [DataAtualizacao] datetime2 NOT NULL,
        [Observacoes] nvarchar(500) NULL,
        CONSTRAINT [PK_StockMotas] PRIMARY KEY ([IDStockMota]),
        CONSTRAINT [FK_StockMotas_ModelosMota_IDModelo] FOREIGN KEY ([IDModelo]) REFERENCES [ModelosMota] ([IDModelo]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_StockMotas_IDModelo] ON [StockMotas]([IDModelo]);
END

IF OBJECT_ID(N'[VendasMotas]') IS NULL
BEGIN
    CREATE TABLE [VendasMotas] (
        [IDVendaMota] int NOT NULL IDENTITY(1, 1),
        [IDModelo] int NOT NULL,
        [IDCliente] int NULL,
        [ClienteNome] nvarchar(150) NOT NULL,
        [ClienteEmail] nvarchar(150) NULL,
        [ClienteTelefone] nvarchar(30) NULL,
        [ClienteNif] nvarchar(50) NULL,
        [NumeroSerie] nvarchar(100) NOT NULL,
        [Cor] nvarchar(60) NOT NULL,
        [Quilometragem] float NOT NULL,
        [Observacoes] nvarchar(500) NULL,
        [DataVenda] datetime2 NOT NULL,
        CONSTRAINT [PK_VendasMotas] PRIMARY KEY ([IDVendaMota]),
        CONSTRAINT [FK_VendasMotas_ModelosMota_IDModelo] FOREIGN KEY ([IDModelo]) REFERENCES [ModelosMota] ([IDModelo]) ON DELETE CASCADE,
        CONSTRAINT [FK_VendasMotas_Clientes_IDCliente] FOREIGN KEY ([IDCliente]) REFERENCES [Clientes] ([IDCliente])
    );

    CREATE INDEX [IX_VendasMotas_IDModelo] ON [VendasMotas]([IDModelo]);
    CREATE INDEX [IX_VendasMotas_IDCliente] ON [VendasMotas]([IDCliente]);
END

IF NOT EXISTS (SELECT 1 FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260319130000_StockMotasVendas')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260319130000_StockMotasVendas', N'8.0.23');
END
