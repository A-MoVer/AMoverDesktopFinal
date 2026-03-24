IF COL_LENGTH('VendasMotas', 'PrecoVenda') IS NULL
    ALTER TABLE VendasMotas ADD PrecoVenda decimal(18,2) NOT NULL DEFAULT 0;

IF COL_LENGTH('VendasMotas', 'CustoAquisicao') IS NULL
    ALTER TABLE VendasMotas ADD CustoAquisicao decimal(18,2) NOT NULL DEFAULT 0;

IF COL_LENGTH('VendasMotas', 'DespesasManutencao') IS NULL
    ALTER TABLE VendasMotas ADD DespesasManutencao decimal(18,2) NULL;
