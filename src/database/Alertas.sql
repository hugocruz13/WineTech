--Inserir Alerta
CREATE OR ALTER PROCEDURE InserirAlerta
    @SensoresId INT,
    @TipoAlerta NVARCHAR(255),
    @Mensagem NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Alertas (SensoresId, TipoAlerta, Mensagem, DataHora, Resolvido)
    VALUES (@SensoresId, @TipoAlerta, @Mensagem, GETDATE(), 0);

    DECLARE @NovoId INT = SCOPE_IDENTITY();

    SELECT Id, SensoresId, TipoAlerta, Mensagem, DataHora, Resolvido
    FROM Alertas
    WHERE Id = @NovoId;
END;
GO

--Obter Alerta por Sensor
CREATE OR ALTER PROCEDURE ObterAlertasPorSensor
    @SensoresId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        SensoresId,
        TipoAlerta,
        Mensagem,
        DataHora,
        Resolvido
    FROM Alertas
    WHERE SensoresId = @SensoresId
END;
GO