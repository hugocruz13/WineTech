use ISI;
go

-- Inserir Adega
CREATE PROCEDURE InserirAdega
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
CREATE PROCEDURE TodasAdegas
AS
BEGIN
    SET NOCOUNT ON;
    SELECT *
    FROM Adega
END;
GO

-- Selecionar uma Adega
CREATE PROCEDURE AdegaById
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
CREATE PROCEDURE ModificarAdega
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

    SELECT *
    FROM Adega
    WHERE Id = @Id;
END;
GO


-- Apagar Adega
CREATE PROCEDURE ApagarAdega
    @Id INT
AS
BEGIN
    SET NOCOUNT OFF;

    DELETE FROM Adega
    WHERE Id = @Id;
END;
GO

-- Stock Resumido
CREATE PROCEDURE ObterResumoPorAdega
    @AdegaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        v.Id AS VinhoId,
        v.Nome,
        v.Produtor,
        v.Ano,
        v.Tipo,
        v.ImagemUrl,
        v.Preco,
        COUNT(s.Id) AS QuantidadeTotal
    FROM Stock s
    INNER JOIN Vinhos v ON s.VinhosId = v.Id
    WHERE s.Adegaid = @AdegaId 
      AND s.Estado = 'Disponivel' 
    GROUP BY 
        v.Id, v.Nome, v.Produtor, v.Ano, v.Tipo, v.ImagemUrl, v.Preco
    ORDER BY 
        v.Nome ASC;
END;
GO

-- Stock todas as adegas
CREATE PROCEDURE ObterResumoStockTotal
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        v.Id AS VinhoId,
        v.Nome,
        v.Produtor,
        v.Ano,
        v.Tipo,
        v.ImagemUrl,
        v.Preco,
        COUNT(s.Id) AS QuantidadeTotal
    FROM Stock s
    INNER JOIN Vinhos v ON s.VinhosId = v.Id
    WHERE s.Estado = 'Disponivel' -- Mantemos este filtro para contar apenas stock disponível
    GROUP BY 
        v.Id, v.Nome, v.Produtor, v.Ano, v.Tipo, v.ImagemUrl, v.Preco
    ORDER BY 
        v.Id ASC;
END;
GO

-- Adicionar stock
CREATE PROCEDURE AdicionarStock
    @AdegaId INT,
    @VinhoId INT,
    @Quantidade INT
AS
BEGIN
    SET NOCOUNT OFF;

    DECLARE @i INT = 0;

    WHILE @i < @Quantidade
    BEGIN
        INSERT INTO Stock (VinhosId, Adegaid, Estado, CreatedAt)
        VALUES (@VinhoId, @AdegaId, 'Disponivel', GETDATE());

        SET @i = @i + 1;
    END
END;
GO

-- Atualizar stock
CREATE PROCEDURE AtualizarStock
    @AdegaId INT,
    @VinhoId INT,
    @Quantidade INT
AS
BEGIN
    SET NOCOUNT OFF;

    DECLARE @StockAtual INT;
    DECLARE @Diferenca INT;
    DECLARE @i INT = 0;

    -- 1. Ver quantas garrafas existem atualmente (Disponíveis)
    SELECT @StockAtual = COUNT(*) 
    FROM Stock 
    WHERE AdegaId = @AdegaId 
      AND VinhosId = @VinhoId 
      AND Estado = 'Disponivel';

    -- A nova quantidade é MAIOR (Adicionar garrafas)
    IF @Quantidade > @StockAtual
    BEGIN
        SET @Diferenca = @Quantidade - @StockAtual;

        WHILE @i < @Diferenca
        BEGIN
            INSERT INTO Stock (VinhosId, Adegaid, Estado, CreatedAt)
            VALUES (@VinhoId, @AdegaId, 'Disponivel', GETDATE());

            SET @i = @i + 1;
        END
    END

    -- A nova quantidade é MENOR (Remover garrafas)
    ELSE IF @Quantidade < @StockAtual
    BEGIN
        SET @Diferenca = @StockAtual - @Quantidade;

        -- Seleciona as garrafas mais antiga que estão disponíveis.
        ;WITH GarrafasParaAtualizar AS (
            SELECT TOP (@Diferenca) *
            FROM Stock
            WHERE AdegaId = @AdegaId 
              AND VinhosId = @VinhoId 
              AND Estado = 'Disponivel'
            ORDER BY CreatedAt ASC 
        )

        UPDATE GarrafasParaAtualizar
        SET Estado = 'Vendido'; 
    END
END;
GO

CREATE or alter PROCEDURE ApagarStockPorVinho
    @VinhoId INT
AS
BEGIN
    SET NOCOUNT OFF;

    DELETE FROM Stock
    WHERE VinhosId = @VinhoId AND Estado = 'Disponivel';
END;
GO

-- Obter Quantidade total
CREATE PROCEDURE ObterOcupacaoAdega
    @AdegaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS Total
    FROM Stock 
    WHERE AdegaId = @AdegaId 
      AND Estado = 'Disponivel';
END;
GO



--Inserir Alerta
CREATE PROCEDURE InserirAlerta
    @SensoresId INT,
    @TipoAlerta NVARCHAR(255),
    @Mensagem NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Alertas (SensoresId, TipoAlerta, Mensagem, DataHora, Resolvido)
    VALUES (@SensoresId, @TipoAlerta, @Mensagem, GETDATE(), 0);

    DECLARE @NovoId INT = SCOPE_IDENTITY();

    SELECT Id, SensoresId, TipoAlerta, Mensagem, DataHora, Resolvido
    FROM Alertas
    WHERE Id = @NovoId;
END;
GO

--Obter Alerta por Sensor
CREATE PROCEDURE ObterAlertasPorSensor
    @SensoresId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        SensoresId,
        TipoAlerta,
        Mensagem,
        DataHora,
        Resolvido
    FROM Alertas
    WHERE SensoresId = @SensoresId
END;
GO

CREATE PROCEDURE GetAllAlertas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        A.Id,
        A.TipoAlerta,
        A.Mensagem,
        A.DataHora,
        A.Resolvido,
        A.SensoresId,
        S.IdentificadorHardware,
        S.Tipo AS TipoSensor,
        S.AdegaId
    FROM Alertas A
    INNER JOIN Sensores S ON A.SensoresId = S.Id
    ORDER BY A.DataHora DESC;
END;
GO

CREATE PROCEDURE ResolverAlerta
    @AlertaId INT
AS
BEGIN
    SET NOCOUNT OFF;

    UPDATE Alertas
    SET Resolvido = 1
    WHERE Id = @AlertaId;
END;
GO


--Obter Carrinho por utilizador
CREATE PROCEDURE ObterCarrinhoPorUtilizador
    @UtilizadoresId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        VinhosId,
        UtilizadoresId,
        Quantidade
    FROM Carrinho
    WHERE UtilizadoresId = @UtilizadoresId
END;
GO

--Adicionar um vinho ao carrinho
CREATE PROCEDURE InserirItemCarrinho
    @UtilizadoresId NVARCHAR(100),
    @VinhosId INT,
    @Quantidade INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM Carrinho
        WHERE UtilizadoresId = @UtilizadoresId
          AND VinhosId = @VinhosId
    )
    BEGIN
        UPDATE Carrinho
        SET Quantidade = @Quantidade
        WHERE UtilizadoresId = @UtilizadoresId
          AND VinhosId = @VinhosId;
    END
    ELSE
    BEGIN
        INSERT INTO Carrinho (UtilizadoresId, VinhosId, Quantidade)
        VALUES (@UtilizadoresId, @VinhosId, @Quantidade);
    END
END;
GO


--Atualizar quantidade 
CREATE PROCEDURE AtualizarItem
    @VinhosId INT,
    @UtilizadoresId NVARCHAR(100),
    @Quantidade INT  
AS
BEGIN
    SET NOCOUNT ON;

    MERGE INTO Carrinho AS Destino
    USING (SELECT @VinhosId AS VinhosId, @UtilizadoresId AS UtilizadoresId, @Quantidade AS Quantidade) AS Fonte
    ON (Destino.VinhosId = Fonte.VinhosId AND Destino.UtilizadoresId = Fonte.UtilizadoresId)

    WHEN MATCHED THEN
        UPDATE SET Destino.Quantidade = Fonte.Quantidade

    WHEN NOT MATCHED THEN
        INSERT (VinhosId, UtilizadoresId, Quantidade)
        VALUES (Fonte.VinhosId, Fonte.UtilizadoresId, Fonte.Quantidade);
END;
GO

--Eliminar vinho de um Carrinho
CREATE PROCEDURE EliminarItem
    @VinhosId INT,
    @UtilizadoresId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT OFF;

    DELETE FROM Carrinho
     WHERE VinhosId = @VinhosId
       AND UtilizadoresId = @UtilizadoresId;
END;
GO

--Eliminar carrinho do utilizador
CREATE PROCEDURE EliminarCarrinho
    @UtilizadorId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT OFF;

    DELETE FROM Carrinho
    WHERE UtilizadoresId = @UtilizadorId;
END;
GO

-- Carrinho por utilizador detalhado
CREATE PROCEDURE ObterDetalhesCarrinho
    @UtilizadoresId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.Id              AS VinhoId,
        v.Nome,
        v.Produtor,
        v.Ano,
        v.Tipo,
        v.Descricao,
        v.ImagemUrl,
        v.Preco,
        SUM(c.Quantidade) AS QuantidadeTotal
    FROM Carrinho c
    INNER JOIN Vinhos v ON v.Id = c.VinhosId
    WHERE c.UtilizadoresId = @UtilizadoresId
    GROUP BY
        v.Id,
        v.Nome,
        v.Produtor,
        v.Ano,
        v.Tipo,
        v.Descricao,
        v.ImagemUrl,
        v.Preco;
END;
GO

-- Seleciona Id's das garrafas fisicas
CREATE PROCEDURE SelecionarStockPorVinho
    @VinhoId INT,
    @Quantidade INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@Quantidade) Id
    FROM Stock
    WHERE VinhosId = @VinhoId
      AND Estado = 'Disponivel'
    ORDER BY CreatedAt ASC; 

END
GO

-- Criar uma compra
CREATE PROCEDURE CriarCompra
    @UtilizadorId nvarchar(100),
    @Estado nvarchar(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NovaCompraId INT;

    INSERT INTO Compras (UtilizadoresId, Estado)
    VALUES (@UtilizadorId, @Estado);

    SET @NovaCompraId = SCOPE_IDENTITY();

    SELECT 
        @NovaCompraId AS Id,
        DataCompra,
        UtilizadoresId,
        ValorTotal,
        Estado,
        Cartao
    FROM Compras
    WHERE Id = @NovaCompraId;
END
GO

-- Criar uma linha 
CREATE PROCEDURE LinhaCompra
    @CompraId INT,
    @StockId INT
AS
BEGIN
    SET NOCOUNT OFF;

    INSERT INTO LinhasCompra (ComprasId, StockId, PrecoUnitario)
    SELECT @CompraId, s.Id, v.Preco
    FROM Stock s
    INNER JOIN Vinhos v ON s.VinhosId = v.Id
    WHERE s.Id = @StockId;
END
GO

-- Atualizar Stock 
CREATE PROCEDURE FinalizarCompra
    @StockId INT
AS
BEGIN
    SET NOCOUNT OFF;

    -- Atualiza a garrafa específica para 'Vendido'
    UPDATE Stock
    SET Estado = 'Vendido'
    WHERE Id = @StockId AND Estado = 'Disponivel';
END
GO

CREATE PROCEDURE AtualizarValorCompra
    @CompraId INT,
    @Valor DECIMAL(19,2),
    @Estado NVARCHAR(100),
    @Cartao int
AS
BEGIN
    SET NOCOUNT OFF;

    UPDATE Compras
    SET 
        ValorTotal = @Valor,
        Estado = @Estado,
        Cartao = @Cartao
    WHERE Id = @CompraId;
END
GO

-- Compras Utilizador
CREATE PROCEDURE ComprasPorUtilizador
    @UtilizadorId nvarchar(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Compras
    WHERE UtilizadoresId = @UtilizadorId
    ORDER BY DataCompra DESC;
END;
GO

-- Compra detalhada
CREATE PROCEDURE DetalhesCompra
    @CompraId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.Id AS CompraId,
        c.DataCompra,
        c.ValorTotal,
        v.Id AS VinhoId,
        v.Nome,
        v.Produtor,
        v.Ano,
        v.Tipo,
        COUNT(*) AS Quantidade,
        v.Preco AS PrecoUnitario,
        v.ImagemUrl,
        u.Nome,
        u.Email,
        u.ImgUrl,
        s.Id AS StockId,
        c.Cartao,
        c.UtilizadoresId
    FROM Compras c
    JOIN LinhasCompra lc ON lc.ComprasId = c.Id
    JOIN Stock s ON s.Id = lc.StockId
    JOIN Vinhos v ON v.Id = s.VinhosId
    JOIN Utilizadores u ON c.UtilizadoresId = u.Id
    WHERE c.Id = @CompraId
    GROUP BY 
        c.Id,
        c.DataCompra,
        c.ValorTotal,
        v.Id,
        v.Nome,
        v.Produtor,
        v.Ano,
        v.Tipo,
        v.Preco,
        v.ImagemUrl,
        u.Nome,
        u.Email,
        u.ImgUrl,
        s.Id ,
        c.Cartao,
        c.UtilizadoresId
END;
GO


--Inserir uma leitura
CREATE PROCEDURE InserirLeitura
    @SensorId INT,
    @Valor FLOAT(10)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Leituras (SensorId, Valor, DataHora)
    VALUES (@SensorId, @Valor, GETDATE());

    DECLARE @NovoId INT = SCOPE_IDENTITY();

    SELECT Id, SensorId, Valor, DataHora
    FROM Leituras
    WHERE Id = @NovoId;
END;
GO

--Obter leituras de um sensor
CREATE PROCEDURE ObterLeiturasPorSensor
    @SensorId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        SensorId,
        Valor,
        DataHora
    FROM Leituras
    WHERE SensorId = @SensorId
    ORDER BY DataHora DESC;
END;
GO

-- Obter leituras por stock
CREATE PROCEDURE ObterLeiturasPorStock
    @StockId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @DataEntradaStock DATETIME2;
    DECLARE @DataCompra DATETIME2;

    -- Data em que a garrafa entrou em stock
    SELECT @DataEntradaStock = CreatedAt
    FROM Stock
    WHERE Id = @StockId;

    -- Data da compra (se j� foi comprada)
    SELECT @DataCompra = MAX(c.DataCompra)
    FROM LinhasCompra lc
    INNER JOIN Compras c ON c.Id = lc.ComprasId
    WHERE lc.StockId = @StockId;

    SELECT
        se.Tipo       AS TipoSensor,
        le.Valor,
        le.DataHora
    FROM Stock st
    INNER JOIN Adega a       ON a.Id = st.AdegaId
    INNER JOIN Sensores se  ON se.AdegaId = a.Id
    INNER JOIN Leituras le  ON le.SensorId = se.Id
    WHERE st.Id = @StockId
      AND le.DataHora >= @DataEntradaStock
      AND le.DataHora <= ISNULL(@DataCompra, SYSDATETIME())
    ORDER BY le.DataHora;
END;
GO

-- Obter leituras por adega
CREATE PROCEDURE ObterLeiturasPorAdega
    @AdegaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        a.Nome        AS NomeAdega,
        se.Id         AS SensorId,
        se.Tipo       AS TipoSensor,
        le.Valor,
        le.DataHora
    FROM Adega a
    INNER JOIN Sensores se ON se.AdegaId = a.Id
    INNER JOIN Leituras le ON le.SensorId = se.Id
    WHERE a.Id = @AdegaId
    ORDER BY le.DataHora;
END;
GO

-- Inserir Notificação
CREATE PROCEDURE InserirNotificacao
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
CREATE PROCEDURE NotificacoesPorUtilizador
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
CREATE PROCEDURE MarcarNotificacaoComoLida
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

--Criar e Associar um Sensor a uma Adega
CREATE PROCEDURE InserirSensor
    @IdentificadorHardware NVARCHAR(255),
    @Tipo NVARCHAR(255),
    @Estado BIT,
    @AdegaId INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Sensores (IdentificadorHardware, Tipo, Estado, AdegaId)
    VALUES (@IdentificadorHardware, @Tipo, @Estado, @AdegaId);

    DECLARE @NovoId INT = SCOPE_IDENTITY();

    SELECT Id, IdentificadorHardware, Tipo, Estado, AdegaId
    FROM Sensores
    WHERE Id = @NovoId;
END;
GO

--Obter todos os Sensores
CREATE PROCEDURE ObterSensores
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        IdentificadorHardware,
        Tipo,
        Estado,
        AdegaId
    FROM Sensores;
END;
GO

--Obter Sensores de uma Adega especifíca
CREATE PROCEDURE ObterSensoresPorAdega
    @AdegaId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        IdentificadorHardware,
        Tipo,
        Estado,
        AdegaId
    FROM Sensores
    WHERE AdegaId = @AdegaId;
END;
GO

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


-- Atualizar Utilizador e retornar o utilizador atualizado
CREATE PROCEDURE ModificarUtilizador
    @Id NVARCHAR(100),
    @Nome NVARCHAR(100) = NULL,
    @Email NVARCHAR(255) = NULL,
    @ImgUrl NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Atualiza apenas os campos que não são nulos
    UPDATE Utilizadores
    SET
        Nome = COALESCE(@Nome, Nome),
        Email = COALESCE(@Email, Email),
        ImgUrl = COALESCE(@ImgUrl, ImgUrl)
    WHERE Id = @Id;

    -- Retorna o utilizador atualizado
    SELECT *
    FROM Utilizadores
    WHERE Id = @Id;
END;
GO

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

