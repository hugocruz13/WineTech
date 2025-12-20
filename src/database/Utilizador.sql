CREATE PROCEDURE RegistrarUtilizador
    @Auth0UserId NVARCHAR(100),
    @Nome NVARCHAR(100) = NULL,
    @Email NVARCHAR(255) = NULL,
    @ImgUrl NVARCHAR(255) = NULL,
    @IsAdmin BIT 
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
            INSERT INTO Utilizadores (Id, Nome, Email, ImgUrl, IsAdmin)
            VALUES (@Auth0UserId, @Nome, @Email, @ImgUrl,@IsAdmin);
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
CREATE PROCEDURE UtilizadorById
    @Auth0UserId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Utilizadores
    WHERE Id = @Auth0UserId;
END;
GO

-- Get apenas utilizadores administradores
CREATE PROCEDURE UtilizadoresAdmin
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Utilizadores
    WHERE IsAdmin = 1;
END;
GO



