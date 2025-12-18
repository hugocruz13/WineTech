--Inserir uma leitura
CREATE OR ALTER PROCEDURE InserirLeitura
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
CREATE OR ALTER PROCEDURE ObterLeiturasPorSensor
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
CREATE OR ALTER PROCEDURE ObterLeiturasPorStock
    @StockId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @DataCompra DATETIME2;

    -- Obter a data da compra associada ao stock
    SELECT @DataCompra = MAX(c.DataCompra)
    FROM LinhasCompra lc
    INNER JOIN Compras c ON c.Id = lc.ComprasId
    WHERE lc.StockId = @StockId;

    SELECT
        se.Tipo AS TipoSensor,
        le.Valor,
        le.DataHora
    FROM Stock st
    INNER JOIN Adega a
        ON a.Id = st.AdegaId
    INNER JOIN Sensores se
        ON se.AdegaId = a.Id
    INNER JOIN Leituras le
        ON le.SensorId = se.Id
    WHERE st.Id = @StockId
      AND le.DataHora >= st.CreatedAt
      AND le.DataHora <= ISNULL(@DataCompra, GETDATE())
    ORDER BY se.Tipo, le.DataHora;
END;
GO

EXEC ObterLeiturasPorStock 3
