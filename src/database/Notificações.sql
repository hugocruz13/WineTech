-- Inserir Notificação
CREATE OR ALTER PROCEDURE InserirNotificacao
    @Titulo NVARCHAR(100),
    @Mensagem NVARCHAR(255),
    @Tipo NVARCHAR(50),
    @UtilizadorId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Notificacoes (Titulo,Mensagem, Tipo,UtilizadoresId)
    VALUES (@Titulo, @Mensagem,@Tipo , @UtilizadorId);

    DECLARE @NovoId INT = SCOPE_IDENTITY();

    SELECT *
    FROM Notificacoes
    WHERE Id = @NovoId;
END;
GO


-- Listar todas as Notificações Por utilizador
CREATE OR ALTER PROCEDURE NotificacoesPorUtilizador
    @UtilizadorId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Notificacoes
    WHERE UtilizadoresId = @UtilizadorId
    ORDER BY CreatedAt DESC;
END;
GO

-- Atualizar estado Notificação
CREATE OR ALTER PROCEDURE MarcarNotificacaoComoLida
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Notificacoes
    SET Lida = 1
    WHERE Id = @Id;

    SELECT *
    FROM Notificacoes
    WHERE Id = @Id;
END;
GO
