--Obter Carrinho por utilizador
CREATE OR ALTER PROCEDURE ObterCarrinhoPorUtilizador
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
CREATE OR ALTER PROCEDURE InserirItemCarrinho
    @UtilizadoresId NVARCHAR(100),
    @VinhosId INT,
    @Quantidade INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Carrinho (UtilizadoresId, VinhosId, Quantidade)
    VALUES (@UtilizadoresId, @VinhosId, @Quantidade);
END;
GO
--Atualizar quantidade 
CREATE OR ALTER PROCEDURE AtualizarItem
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