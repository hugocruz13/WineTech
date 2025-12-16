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
