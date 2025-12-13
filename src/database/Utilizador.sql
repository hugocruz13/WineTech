SELECT name
FROM sys.procedures

-- Inserir utilizador
CREATE OR ALTER PROCEDURE RegistrarUtilizador
    @Auth0UserId NVARCHAR(100),
    @Nome NVARCHAR(100) = NULL,
    @Email NVARCHAR(255) = NULL,
    @ImgUrl NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Inserir o utilizador, permitindo NULL nos campos opcionais
    INSERT INTO Utilizadores (Id, Nome, Email, ImgUrl)
    VALUES (@Auth0UserId, @Nome, @Email,@ImgUrl);

    -- Retorna o utilizador criado
    SELECT *
    FROM Utilizadores
    WHERE Id = @Auth0UserId;
END;
GO

-- Get Utilizador pelo ID
CREATE OR ALTER PROCEDURE UtilizadorById
    @Auth0UserId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Utilizadores
    WHERE Id = @Auth0UserId;
END;
GO

