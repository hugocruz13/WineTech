-- Inserir Adega
CREATE OR ALTER PROCEDURE InserirAdega
    @Localizacao NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Adega (Localizacao)
    VALUES (@Localizacao);

    SELECT SCOPE_IDENTITY() AS Id;
END;
GO

 
-- Selecionar todas as Adegas
CREATE OR ALTER PROCEDURE TodasAdegas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Localizacao
    FROM Adega
END;
GO

-- Selecionar uma Adega
CREATE OR ALTER PROCEDURE AdegaById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Localizacao
    FROM Adega
    WHERE Id = @Id;
END;
GO

-- Atualizar Adega 
CREATE OR ALTER PROCEDURE ModificarAdega
    @Id INT,
    @Localizacao NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT OFF;

    UPDATE Adega
       SET Localizacao = @Localizacao
     WHERE Id = @Id;
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