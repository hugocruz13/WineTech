-- Seleciona Id's das garrafas fisicas
CREATE PROCEDURE SelecionarStockPorVinho
    @VinhoId INT,
    @Quantidade INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@Quantidade) Id
    FROM Stock
    WHERE VinhosId = @VinhoId
      AND Estado = 'Disponivel'
    ORDER BY CreatedAt ASC; 

END
GO

-- Criar uma compra
CREATE PROCEDURE CriarCompra
    @UtilizadorId nvarchar(100),
    @Estado nvarchar(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NovaCompraId INT;

    INSERT INTO Compras (UtilizadoresId, Estado)
    VALUES (@UtilizadorId, @Estado);

    SET @NovaCompraId = SCOPE_IDENTITY();

    SELECT 
        @NovaCompraId AS Id,
        DataCompra,
        UtilizadoresId,
        ValorTotal,
        Estado,
        Cartao
    FROM Compras
    WHERE Id = @NovaCompraId;
END
GO

-- Criar uma linha 
CREATE PROCEDURE LinhaCompra
    @CompraId INT,
    @StockId INT
AS
BEGIN
    SET NOCOUNT OFF;

    INSERT INTO LinhasCompra (ComprasId, StockId, PrecoUnitario)
    SELECT @CompraId, s.Id, v.Preco
    FROM Stock s
    INNER JOIN Vinhos v ON s.VinhosId = v.Id
    WHERE s.Id = @StockId;
END
GO

-- Atualizar Stock 
CREATE PROCEDURE FinalizarCompra
    @StockId INT
AS
BEGIN
    SET NOCOUNT OFF;

    -- Atualiza a garrafa específica para 'Vendido'
    UPDATE Stock
    SET Estado = 'Vendido'
    WHERE Id = @StockId AND Estado = 'Disponivel';
END
GO

CREATE PROCEDURE AtualizarValorCompra
    @CompraId INT,
    @Valor DECIMAL(19,2),
    @Estado NVARCHAR(100),
    @Cartao int
AS
BEGIN
    SET NOCOUNT OFF;

    UPDATE Compras
    SET 
        ValorTotal = @Valor,
        Estado = @Estado,
        Cartao = @Cartao
    WHERE Id = @CompraId;
END
GO

-- Compras Utilizador
CREATE PROCEDURE ComprasPorUtilizador
    @UtilizadorId nvarchar(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Compras
    WHERE UtilizadoresId = @UtilizadorId
    ORDER BY DataCompra DESC;
END;
GO

-- Compra detalhada
CREATE PROCEDURE DetalhesCompra
    @CompraId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.Id AS CompraId,
        c.DataCompra,
        c.ValorTotal,
        v.Id AS VinhoId,
        v.Nome,
        v.Produtor,
        v.Ano,
        v.Tipo,
        COUNT(*) AS Quantidade,
        v.Preco AS PrecoUnitario,
        v.ImagemUrl,
        u.Nome,
        u.Email,
        u.ImgUrl,
        s.Id AS StockId,
        c.Cartao,
        c.UtilizadoresId
    FROM Compras c
    JOIN LinhasCompra lc ON lc.ComprasId = c.Id
    JOIN Stock s ON s.Id = lc.StockId
    JOIN Vinhos v ON v.Id = s.VinhosId
    JOIN Utilizadores u ON c.UtilizadoresId = u.Id
    WHERE c.Id = @CompraId
    GROUP BY 
        c.Id,
        c.DataCompra,
        c.ValorTotal,
        v.Id,
        v.Nome,
        v.Produtor,
        v.Ano,
        v.Tipo,
        v.Preco,
        v.ImagemUrl,
        u.Nome,
        u.Email,
        u.ImgUrl,
        s.Id ,
        c.Cartao,
        c.UtilizadoresId
END;
GO
