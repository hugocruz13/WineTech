--Obter Carrinho por utilizador
CREATE PROCEDURE ObterCarrinhoPorUtilizador
    @UtilizadoresId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        VinhosId,
        UtilizadoresId,
        Quantidade
    FROM Carrinho
    WHERE UtilizadoresId = @UtilizadoresId
END;
GO

--Adicionar um vinho ao carrinho
CREATE PROCEDURE InserirItemCarrinho
    @UtilizadoresId NVARCHAR(100),
    @VinhosId INT,
    @Quantidade INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM Carrinho
        WHERE UtilizadoresId = @UtilizadoresId
          AND VinhosId = @VinhosId
    )
    BEGIN
        UPDATE Carrinho
        SET Quantidade = @Quantidade
        WHERE UtilizadoresId = @UtilizadoresId
          AND VinhosId = @VinhosId;
    END
    ELSE
    BEGIN
        INSERT INTO Carrinho (UtilizadoresId, VinhosId, Quantidade)
        VALUES (@UtilizadoresId, @VinhosId, @Quantidade);
    END
END;
GO


--Atualizar quantidade 
CREATE PROCEDURE AtualizarItem
    @VinhosId INT,
    @UtilizadoresId NVARCHAR(100),
    @Quantidade INT  
AS
BEGIN
    SET NOCOUNT ON;

    MERGE INTO Carrinho AS Destino
    USING (SELECT @VinhosId AS VinhosId, @UtilizadoresId AS UtilizadoresId, @Quantidade AS Quantidade) AS Fonte
    ON (Destino.VinhosId = Fonte.VinhosId AND Destino.UtilizadoresId = Fonte.UtilizadoresId)

    WHEN MATCHED THEN
        UPDATE SET Destino.Quantidade = Fonte.Quantidade

    WHEN NOT MATCHED THEN
        INSERT (VinhosId, UtilizadoresId, Quantidade)
        VALUES (Fonte.VinhosId, Fonte.UtilizadoresId, Fonte.Quantidade);
END;
GO

--Eliminar vinho de um Carrinho
CREATE PROCEDURE EliminarItem
    @VinhosId INT,
    @UtilizadoresId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT OFF;

    DELETE FROM Carrinho
     WHERE VinhosId = @VinhosId
       AND UtilizadoresId = @UtilizadoresId;
END;
GO

--Eliminar carrinho do utilizador
CREATE PROCEDURE EliminarCarrinho
    @UtilizadorId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT OFF;

    DELETE FROM Carrinho
    WHERE UtilizadoresId = @UtilizadorId;
END;
GO

-- Carrinho por utilizador detalhado
CREATE PROCEDURE ObterDetalhesCarrinho
    @UtilizadoresId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.Id              AS VinhoId,
        v.Nome,
        v.Produtor,
        v.Ano,
        v.Tipo,
        v.Descricao,
        v.ImagemUrl,
        v.Preco,
        SUM(c.Quantidade) AS QuantidadeTotal
    FROM Carrinho c
    INNER JOIN Vinhos v ON v.Id = c.VinhosId
    WHERE c.UtilizadoresId = @UtilizadoresId
    GROUP BY
        v.Id,
        v.Nome,
        v.Produtor,
        v.Ano,
        v.Tipo,
        v.Descricao,
        v.ImagemUrl,
        v.Preco;
END;
GO
