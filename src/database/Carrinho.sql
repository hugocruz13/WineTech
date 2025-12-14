--Obter Carrinho por utilizador
CREATE OR ALTER PROCEDURE ObterCarrinho
    @UtilizadoresId INT
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