SELECT name
FROM sys.procedures

CREATE OR ALTER PROCEDURE RegistrarUtilizador
    @Auth0UserId NVARCHAR(100),
    @Nome NVARCHAR(100) = NULL,
    @Email NVARCHAR(255) = NULL,
    @ImgUrl NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (
            SELECT 1
            FROM Utilizadores WITH (UPDLOCK, HOLDLOCK)
            WHERE Id = @Auth0UserId
        )
        BEGIN
            INSERT INTO Utilizadores (Id, Nome, Email, ImgUrl)
            VALUES (@Auth0UserId, @Nome, @Email, @ImgUrl);
        END
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() NOT IN (2627, 2601)
            THROW;
    END CATCH

    -- Retorna sempre o utilizador (exista ou tenha acabado de ser inserido)
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

