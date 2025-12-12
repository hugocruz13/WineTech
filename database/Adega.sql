-- Inserir Adega
CREATE OR ALTER PROCEDURE InserirAdega
    @Nome NVARCHAR(100),
    @Localizacao NVARCHAR(255),
    @Capacidade INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Adega (Nome, Localizacao, Capacidade)
    VALUES (@Nome, @Localizacao, @Capacidade);

    DECLARE @NovoId INT = SCOPE_IDENTITY();

    SELECT *
    FROM Adega
    WHERE Id = @NovoId;
END;
GO

 
-- Selecionar todas as Adegas
CREATE OR ALTER PROCEDURE TodasAdegas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM Adega
END;
GO

-- Selecionar uma Adega
CREATE OR ALTER PROCEDURE AdegaById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Adega
    WHERE Id = @Id;
END;
GO

-- Atualizar Adega e retornar a adega atualizada
CREATE OR ALTER PROCEDURE ModificarAdega
    @Id INT,
    @Nome NVARCHAR(100) = NULL,
    @Localizacao NVARCHAR(255) = NULL,
    @Capacidade INT = NULL,
    @ImagemUrl NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Atualiza apenas os campos que não são nulos
    UPDATE Adega
    SET 
        Nome = COALESCE(@Nome, Nome),
        Localizacao = COALESCE(@Localizacao, Localizacao),
        Capacidade = COALESCE(@Capacidade, Capacidade),
        ImagemUrl = COALESCE(@ImagemUrl, ImagemUrl)
    WHERE Id = @Id;

    SELECT *
    FROM Adega
    WHERE Id = @Id;
END;
GO


-- Apagar Adega
CREATE OR ALTER PROCEDURE ApagarAdega
    @Id INT
AS
BEGIN
    SET NOCOUNT OFF;

    DELETE FROM Adega
    WHERE Id = @Id;
END;

-- Stock Resumido
CREATE OR ALTER PROCEDURE ObterResumoPorAdega
    @AdegaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        v.Id AS VinhoId,
        v.Nome,
        v.Produtor,
        v.Ano,
        v.Tipo,
        v.ImagemUrl,
        v.Preco,
        COUNT(s.Id) AS QuantidadeTotal
    FROM Stock s
    INNER JOIN Vinhos v ON s.VinhosId = v.Id
    WHERE s.Adegaid = @AdegaId 
      AND s.Estado = 'Disponivel' 
    GROUP BY 
        v.Id, v.Nome, v.Produtor, v.Ano, v.Tipo, v.ImagemUrl, v.Preco
    ORDER BY 
        v.Nome ASC;
END;
GO

-- Adicionar stock
CREATE OR ALTER PROCEDURE AdicionarStock
    @AdegaId INT,
    @VinhoId INT,
    @Quantidade INT
AS
BEGIN
    SET NOCOUNT OFF;

    DECLARE @i INT = 0;

    WHILE @i < @Quantidade
    BEGIN
        INSERT INTO Stock (VinhosId, Adegaid, Estado, CreatedAt)
        VALUES (@VinhoId, @AdegaId, 'Disponivel', GETDATE());

        SET @i = @i + 1;
    END
END;
GO

-- Atualizar stock
CREATE OR ALTER PROCEDURE AtualizarStock
    @AdegaId INT,
    @VinhoId INT,
    @Quantidade INT
AS
BEGIN
    SET NOCOUNT OFF;

    DECLARE @StockAtual INT;
    DECLARE @Diferenca INT;
    DECLARE @i INT = 0;

    -- 1. Ver quantas garrafas existem atualmente (Disponíveis)
    SELECT @StockAtual = COUNT(*) 
    FROM Stock 
    WHERE AdegaId = @AdegaId 
      AND VinhosId = @VinhoId 
      AND Estado = 'Disponivel';

    -- A nova quantidade é MAIOR (Adicionar garrafas)
    IF @Quantidade > @StockAtual
    BEGIN
        SET @Diferenca = @Quantidade - @StockAtual;

        WHILE @i < @Diferenca
        BEGIN
            INSERT INTO Stock (VinhosId, Adegaid, Estado, CreatedAt)
            VALUES (@VinhoId, @AdegaId, 'Disponivel', GETDATE());

            SET @i = @i + 1;
        END
    END

    -- A nova quantidade é MENOR (Remover garrafas)
    ELSE IF @Quantidade < @StockAtual
    BEGIN
        SET @Diferenca = @StockAtual - @Quantidade;

        -- Seleciona as garrafas mais antiga que estão disponíveis.
        ;WITH GarrafasParaAtualizar AS (
            SELECT TOP (@Diferenca) *
            FROM Stock
            WHERE AdegaId = @AdegaId 
              AND VinhosId = @VinhoId 
              AND Estado = 'Disponivel'
            ORDER BY CreatedAt ASC 
        )

        UPDATE GarrafasParaAtualizar
        SET Estado = 'Vendido'; 
    END
END;
GO

-- Obter Quantidade total
CREATE OR ALTER PROCEDURE ObterOcupacaoAdega
    @AdegaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS Total
    FROM Stock 
    WHERE AdegaId = @AdegaId 
      AND Estado = 'Disponivel';
END;
GO