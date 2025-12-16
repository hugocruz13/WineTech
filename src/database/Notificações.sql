-- Inserir Notificação
CREATE OR ALTER PROCEDURE InserirNotificacao
    @Mensagem NVARCHAR(255),
    @UtilizadorId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Notificacoes (Mensagem, UtilizadoresId)
    VALUES (@Mensagem, @UtilizadorId);

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
    SET NOCOUNT OFF;

    UPDATE Notificacoes
    SET Lida = 1
    WHERE Id = @Id;
END;
GO
