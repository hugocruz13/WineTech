--Inserir Alerta
CREATE PROCEDURE InserirAlerta
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
CREATE PROCEDURE ObterAlertasPorSensor
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

CREATE PROCEDURE GetAllAlertas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        A.Id,
        A.TipoAlerta,
        A.Mensagem,
        A.DataHora,
        A.Resolvido,
        A.SensoresId,
        S.IdentificadorHardware,
        S.Tipo AS TipoSensor,
        S.AdegaId
    FROM Alertas A
    INNER JOIN Sensores S ON A.SensoresId = S.Id
    ORDER BY A.DataHora DESC;
END;
GO

CREATE PROCEDURE ResolverAlerta
    @AlertaId INT
AS
BEGIN
    SET NOCOUNT OFF;

    UPDATE Alertas
    SET Resolvido = 1
    WHERE Id = @AlertaId;
END;
GO
