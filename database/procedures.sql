SELECT name
FROM sys.procedures

CREATE OR ALTER PROCEDURE RegistrarUtilizador
    @Auth0UserId NVARCHAR(100),
    @Nome NVARCHAR(100),
    @Email NVARCHAR(255),
    @ImgUrl NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @NovoId INT;

    INSERT INTO Utilizadores (Auth0UserId, Nome, Email, ImgUrl)
    VALUES (@Auth0UserId, @Nome, @Email, @ImgUrl);

    SET @NovoId = SCOPE_IDENTITY();

    SELECT @NovoId AS Id;
END;