--Inserir uma leitura
CREATE PROCEDURE InserirLeitura
    @SensorId INT,
    @Valor FLOAT(10)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Leituras (SensorId, Valor, DataHora)
    VALUES (@SensorId, @Valor, GETDATE());

    DECLARE @NovoId INT = SCOPE_IDENTITY();

    SELECT Id, SensorId, Valor, DataHora
    FROM Leituras
    WHERE Id = @NovoId;
END;
GO

--Obter leituras de um sensor
CREATE PROCEDURE ObterLeiturasPorSensor
    @SensorId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        SensorId,
        Valor,
        DataHora
    FROM Leituras
    WHERE SensorId = @SensorId
    ORDER BY DataHora DESC;
END;
GO

-- Obter leituras por stock
CREATE PROCEDURE ObterLeiturasPorStock
    @StockId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @DataEntradaStock DATETIME2;
    DECLARE @DataCompra DATETIME2;

    -- Data em que a garrafa entrou em stock
    SELECT @DataEntradaStock = CreatedAt
    FROM Stock
    WHERE Id = @StockId;

    -- Data da compra (se j� foi comprada)
    SELECT @DataCompra = MAX(c.DataCompra)
    FROM LinhasCompra lc
    INNER JOIN Compras c ON c.Id = lc.ComprasId
    WHERE lc.StockId = @StockId;

    SELECT
        se.Tipo       AS TipoSensor,
        le.Valor,
        le.DataHora
    FROM Stock st
    INNER JOIN Adega a       ON a.Id = st.AdegaId
    INNER JOIN Sensores se  ON se.AdegaId = a.Id
    INNER JOIN Leituras le  ON le.SensorId = se.Id
    WHERE st.Id = @StockId
      AND le.DataHora >= @DataEntradaStock
      AND le.DataHora <= ISNULL(@DataCompra, SYSDATETIME())
    ORDER BY le.DataHora;
END;
GO
