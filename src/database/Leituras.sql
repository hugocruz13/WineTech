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