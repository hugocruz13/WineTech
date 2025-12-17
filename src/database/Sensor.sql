--Criar e Associar um Sensor a uma Adega
CREATE OR ALTER PROCEDURE InserirSensor
    @IdentificadorHardware NVARCHAR(255),
    @Tipo NVARCHAR(255),
    @Estado BIT,
    @AdegaId INT,
    @ImagemUrl NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Sensores (IdentificadorHardware, Tipo, Estado, AdegaId, ImagemUrl)
    VALUES (@IdentificadorHardware, @Tipo, @Estado, @AdegaId, @ImagemUrl);

    DECLARE @NovoId INT = SCOPE_IDENTITY();

    SELECT Id, IdentificadorHardware, Tipo, Estado, AdegaId, ImagemUrl
    FROM Sensores
    WHERE Id = @NovoId;
END;
GO


--Obter todos os Sensores
CREATE OR ALTER PROCEDURE ObterSensores
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        IdentificadorHardware,
        Tipo,
        Estado,
        ImagemUrl,
        AdegaId
    FROM Sensores;
END;
GO

--Obter Sensores de uma Adega especifíca
CREATE OR ALTER PROCEDURE ObterSensoresPorAdega
    @AdegaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        IdentificadorHardware,
        Tipo,
        Estado,
        ImagemUrl,
        AdegaId
    FROM Sensores
    WHERE AdegaId = @AdegaId;
END;
GO


