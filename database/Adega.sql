-- Inserir Adega
CREATE OR ALTER PROCEDURE InserirAdega
    @Nome NVARCHAR(100),
    @Localizacao NVARCHAR(255),
    @Capacidade INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Adega (Nome, Localizacao, Capacidade)
    VALUES (@Nome, @Localizacao, @Capacidade);

    DECLARE @NovoId INT = SCOPE_IDENTITY();

    SELECT *
    FROM Adega
    WHERE Id = @NovoId;
END;
GO

 
-- Selecionar todas as Adegas
CREATE OR ALTER PROCEDURE TodasAdegas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM Adega
END;
GO

-- Selecionar uma Adega
CREATE OR ALTER PROCEDURE AdegaById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Adega
    WHERE Id = @Id;
END;
GO

-- Atualizar Adega e retornar a adega atualizada
CREATE OR ALTER PROCEDURE ModificarAdega
    @Id INT,
    @Nome NVARCHAR(100) = NULL,
    @Localizacao NVARCHAR(255) = NULL,
    @Capacidade INT = NULL,
    @ImagemUrl NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Atualiza apenas os campos que não são nulos
    UPDATE Adega
    SET 
        Nome = COALESCE(@Nome, Nome),
        Localizacao = COALESCE(@Localizacao, Localizacao),
        Capacidade = COALESCE(@Capacidade, Capacidade),
        ImagemUrl = COALESCE(@ImagemUrl, ImagemUrl)
    WHERE Id = @Id;

    -- Retorna o registro atualizado
    SELECT * FROM Adega WHERE Id = @Id;
END;


-- Apagar Adega
CREATE OR ALTER PROCEDURE ApagarAdega
    @Id INT
AS
BEGIN
    SET NOCOUNT OFF;

    DELETE FROM Adega
    WHERE Id = @Id;
END;


DROP Procedure InserirAdega;
EXEC InserirAdega @Localizacao = 'Braga - Portugal';
EXEC TodasAdegas;
EXEC AdegaById @Id = 1;