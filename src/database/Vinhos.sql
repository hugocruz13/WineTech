-- Inserir Vinhos
CREATE PROCEDURE InserirVinho
	@Nome NVARCHAR(255),
    @Produtor NVARCHAR(255),
    @Ano INT,
    @Tipo NVARCHAR(255),
    @Descricao NVARCHAR(255),
    @Preco FLOAT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Vinhos (Nome, Produtor, Ano, Tipo, Descricao, Preco)
    VALUES (@Nome, @Produtor, @Ano, @Tipo, @Descricao, @Preco)

    DECLARE @NovoId INT = SCOPE_IDENTITY();

    SELECT *
    FROM Vinhos
    WHERE Id = @NovoId;
END;
GO

-- Selecionar todos os Vinhos
CREATE PROCEDURE TodosVinhos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nome, Produtor, Ano, Tipo, Descricao, ImagemURL, Preco
    FROM Vinhos
END;
GO

-- Selecionar um Vinho
CREATE PROCEDURE VinhoById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Vinhos
    WHERE Id = @Id;
END;
GO

-- Atualizar Vinho
CREATE PROCEDURE ModificarVinho
    @Id INT,
    @Nome NVARCHAR(255) = NULL,
    @Produtor NVARCHAR(255) = NULL,
    @Ano INT,
    @Tipo NVARCHAR(255) = NULL,
    @Descricao NVARCHAR(255) = NULL,
    @ImagemUrl NVARCHAR(255) = NULL,
    @Preco FLOAT
AS
BEGIN
    SET NOCOUNT OFF;

    UPDATE Vinhos
       SET 
           Nome = COALESCE (@Nome, Nome),
           Produtor = COALESCE (@Produtor, Produtor),
           Ano = COALESCE (@Ano, Ano),
           Tipo = COALESCE (@Tipo, Tipo),
           Descricao = COALESCE (@Descricao, Descricao),
           ImagemUrl = COALESCE (@ImagemUrl, ImagemUrl),
           Preco = COALESCE (@Preco, Preco)
     WHERE Id = @Id;

     SELECT *
     FROM Vinhos
     WHERE Id = @Id;
END;
GO

-- Apagar Vinho
CREATE PROCEDURE ApagarVinho
    @Id INT
AS
BEGIN
    SET NOCOUNT OFF;

    DELETE FROM Vinhos
    WHERE Id = @Id;
END;
GO
