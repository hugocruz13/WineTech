USE ISI;

INSERT INTO [ISI].[dbo].[Adega] (Nome, Localizacao, Capacidade, ImagemUrl)
VALUES
    (N'Adega do Vale', N'Douro', 500, N'https://vinsel.com.br/cdn/shop/articles/organizar-vinhos-na-adega.jpg?v=1727459351'),
    (N'Quinta da Serra', N'Dão', 300, N'https://revistaadega.uol.com.br/media/residencial35.jpg');

/* ============================================================
   TABELA: VINHOS (12 vinhos)
   ============================================================ */

INSERT INTO [ISI].[dbo].[Vinhos]
    (Nome, Produtor, Ano, Tipo, Descricao, ImagemUrl, Preco)
VALUES
    -- Adega 1 ----------------------------------------------------
    (N'Reserva do Vale', N'Adega do Vale', 2018, N'Tinto',
     N'Vinho tinto encorpado e elegante, com aromas intensos a frutos vermelhos maduros, notas subtis de especiarias e taninos bem integrados. Final persistente e equilibrado, ideal para acompanhar carnes vermelhas e pratos tradicionais.',
     N'https://www.casaamerico.pt/wp-content/uploads/2024/10/Quinta-do-Vale-reserva-tinto.jpg', 18.50),

    (N'Rosé do Vale', N'Adega do Vale', 2021, N'Rosé',
     N'Rosé fresco e aromático, com notas delicadas de frutos vermelhos e florais. Leve, vibrante e muito fácil de beber, perfeito para dias quentes, aperitivos ou refeições leves.',
     N'https://www.almadeportugal.com/cdn/shop/files/qta-vale-tinto-reserva.png?v=1712760543', 9.90),

    (N'Branco do Vale Reserva', N'Adega do Vale', 2022, N'Branco',
     N'Branco muito aromático e expressivo, com notas de fruta branca madura, citrinos e ligeiro toque mineral. Na boca é fresco, equilibrado e persistente, ideal para peixe, marisco e saladas.',
     N'https://cdnx.jumpseller.com/selectmoment/image/46760355/vllvr.jpg?1711042103', 14.30),

    (N'Superior do Vale', N'Adega do Vale', 2017, N'Tinto',
     N'Vinho tinto de grande estrutura e complexidade, com notas de fruta preta madura, madeira bem integrada e especiarias. Taninos firmes e final longo, ideal para pratos intensos e momentos especiais.',
     N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRn8_-iYmW1K8YmyvFpXea8-OJ4zk-jMOMkQQ&s', 25.50),

    (N'Vale Late Harvest', N'Adega do Vale', 2021, N'Doce',
     N'Vinho doce elegante, com doçura equilibrada por uma acidez fresca. Apresenta aromas de fruta madura, mel e notas florais. Excelente para sobremesas, queijos ou como vinho de contemplação.',
     N'https://www.portugalvineyards.com/124018-large_default/les-a-les-late-harvest-2021.jpg', 19.90),

    (N'Espumante Vale Bruto', N'Adega do Vale', 2020, N'Espumante',
     N'Espumante fresco e elegante, com bolha fina e persistente. Aromas florais e frutados, boca equilibrada e refrescante. Ideal para celebrações, aperitivos e momentos especiais.',
     N'https://bonsbagos.com/cdn/shop/files/quinta-vale-d_aldeia-espumante-bruto-nature.jpg?v=1746819127&width=1800', 11.80),

    -- Adega 2 ----------------------------------------------------
    (N'Branco da Serra', N'Quinta da Serra', 2020, N'Branco',
     N'Branco fresco e mineral, com aromas cítricos e notas florais subtis. Na boca apresenta boa acidez e equilíbrio, sendo perfeito para pratos leves, peixe grelhado e marisco.',
     N'https://garrafeiracopocheio.pt/908-home_default/quinta-serra-doura-reserva-branco-2017.jpg', 12.00),

    (N'Tinto da Serra', N'Quinta da Serra', 2019, N'Tinto',
     N'Vinho tinto harmonioso, com aromas a frutos pretos maduros e ligeiro toque especiado. Taninos macios e final suave, ideal para consumo diário e refeições tradicionais.',
     N'https://www.portugalvineyards.com/95670/quinta-serra-doura-reserve-white-2017.jpg', 15.20),

    (N'Serra Reserva Especial', N'Quinta da Serra', 2016, N'Tinto',
     N'Tinto complexo e sofisticado, com notas profundas de fruta madura, especiarias e madeira bem integrada. Taninos sedosos e final longo e elegante. Ideal para ocasiões especiais.',
     N'https://garrafeiracopocheio.pt/905-home_default/quinta-serra-doura-reserva-tinto-2016.jpg', 27.00),

    (N'Serra Rosé Premium', N'Quinta da Serra', 2022, N'Rosé',
     N'Rosé premium leve e aromático, com notas de frutos silvestres e excelente frescura. Elegante e equilibrado, perfeito para acompanhar pratos leves ou desfrutar a solo.',
     N'https://www.portugalvineyards.com/95673-home_default/quinta-serra-doura-reserve-rose-2017.jpg', 10.50),

    (N'Serra Colheita Selecionada', N'Quinta da Serra', 2021, N'Branco',
     N'Branco expressivo com aromas cítricos e notas minerais elegantes. Boca fresca, equilibrada e persistente, ideal para peixe, marisco e cozinha mediterrânica.',
     N'https://www.vinalda.pt/media/productimages/172.009_serramaecolheitabranco.png', 13.40),

    (N'Serra Licoroso', N'Quinta da Serra', 2019, N'Licoroso',
     N'Vinho licoroso doce e envolvente, com aromas intensos e final longo e persistente. Excelente para acompanhar sobremesas, queijos ou para momentos de degustação tranquila.',
     N'https://www.portugalvineyards.com/128962-home_default/quinta-serra-doura-touriga-franca-grand-reserve-red-2019.jpg', 18.00);

/* ============================================================
   TABELA: STOCK (ligação Vinhos <-> Adegas)
   ============================================================ */

INSERT INTO [ISI].[dbo].[Stock] (VinhosId, AdegaId)
VALUES
    -- Adega 1 (3 unidades por vinho)
    (1, 1), (1, 1), (1, 1),
    (2, 1), (2, 1), (2, 1),
    (3, 1), (3, 1), (3, 1),
    (4, 1), (4, 1), (4, 1),
    (5, 1), (5, 1), (5, 1),
    (6, 1), (6, 1), (6, 1),

    -- Adega 2 (3 unidades por vinho)
    (7, 2), (7, 2), (7, 2),
    (8, 2), (8, 2), (8, 2),
    (9, 2), (9, 2), (9, 2),
    (10, 2), (10, 2), (10, 2),
    (11, 2), (11, 2), (11, 2),
    (12, 2), (12, 2), (12, 2);

INSERT INTO [ISI].[dbo].[Sensores] (IdentificadorHardware, Tipo, Estado, Adegaid)
VALUES
    (N'REAL_ADEGA_1', N'Temperatura', 1, 1),
    (N'REAL_ADEGA_1', N'Humidade', 1, 1),
    (N'REAL_ADEGA_1', N'Luminosidade', 1, 1),
    (N'GERADO', N'Temperatura', 1, 2),
    (N'GERADO', N'Humidade', 1, 2),
    (N'GERADO', N'Luminosidade', 1, 2);

/* ============================================================
    TABELA: LEITURAS (temperatura, humidade e luminosidade)
    ============================================================ */

DECLARE @SensorTempAdega1 INT;
DECLARE @SensorHumAdega1 INT;
DECLARE @SensorLumAdega1 INT;
DECLARE @SensorTempAdega2 INT;
DECLARE @SensorHumAdega2 INT;
DECLARE @SensorLumAdega2 INT;

SELECT TOP 1 @SensorTempAdega1 = Id
FROM [ISI].[dbo].[Sensores]
WHERE AdegaId = 1 AND Tipo = N'Temperatura';

SELECT TOP 1 @SensorHumAdega1 = Id
FROM [ISI].[dbo].[Sensores]
WHERE AdegaId = 1 AND Tipo = N'Humidade';

SELECT TOP 1 @SensorLumAdega1 = Id
FROM [ISI].[dbo].[Sensores]
WHERE AdegaId = 1 AND Tipo = N'Luminosidade';

SELECT TOP 1 @SensorTempAdega2 = Id
FROM [ISI].[dbo].[Sensores]
WHERE AdegaId = 2 AND Tipo = N'Temperatura';

SELECT TOP 1 @SensorHumAdega2 = Id
FROM [ISI].[dbo].[Sensores]
WHERE AdegaId = 2 AND Tipo = N'Humidade';

SELECT TOP 1 @SensorLumAdega2 = Id
FROM [ISI].[dbo].[Sensores]
WHERE AdegaId = 2 AND Tipo = N'Luminosidade';

INSERT INTO [ISI].[dbo].[Leituras] (SensorId, Valor, DataHora)
VALUES
     -- Adega 1 - Temperatura (C)
     (@SensorTempAdega1, 13.8, DATEADD(HOUR, -12, GETDATE())),
     (@SensorTempAdega1, 14.1, DATEADD(HOUR, -10, GETDATE())),
     (@SensorTempAdega1, 13.9, DATEADD(HOUR, -8, GETDATE())),
     (@SensorTempAdega1, 14.3, DATEADD(HOUR, -6, GETDATE())),
     (@SensorTempAdega1, 14.0, DATEADD(HOUR, -4, GETDATE())),
     (@SensorTempAdega1, 13.7, DATEADD(HOUR, -2, GETDATE())),

     -- Adega 1 - Humidade (%)
     (@SensorHumAdega1, 64.0, DATEADD(HOUR, -12, GETDATE())),
     (@SensorHumAdega1, 65.2, DATEADD(HOUR, -10, GETDATE())),
     (@SensorHumAdega1, 63.8, DATEADD(HOUR, -8, GETDATE())),
     (@SensorHumAdega1, 66.1, DATEADD(HOUR, -6, GETDATE())),
     (@SensorHumAdega1, 64.9, DATEADD(HOUR, -4, GETDATE())),
     (@SensorHumAdega1, 65.5, DATEADD(HOUR, -2, GETDATE())),

     -- Adega 1 - Luminosidade (lux)
     (@SensorLumAdega1, 110.0, DATEADD(HOUR, -12, GETDATE())),
     (@SensorLumAdega1, 95.0, DATEADD(HOUR, -10, GETDATE())),
     (@SensorLumAdega1, 80.0, DATEADD(HOUR, -8, GETDATE())),
     (@SensorLumAdega1, 60.0, DATEADD(HOUR, -6, GETDATE())),
     (@SensorLumAdega1, 45.0, DATEADD(HOUR, -4, GETDATE())),
     (@SensorLumAdega1, 35.0, DATEADD(HOUR, -2, GETDATE())),

     -- Adega 2 - Temperatura (C)
     (@SensorTempAdega2, 15.2, DATEADD(HOUR, -12, GETDATE())),
     (@SensorTempAdega2, 15.5, DATEADD(HOUR, -10, GETDATE())),
     (@SensorTempAdega2, 15.1, DATEADD(HOUR, -8, GETDATE())),
     (@SensorTempAdega2, 15.4, DATEADD(HOUR, -6, GETDATE())),
     (@SensorTempAdega2, 15.0, DATEADD(HOUR, -4, GETDATE())),
     (@SensorTempAdega2, 14.8, DATEADD(HOUR, -2, GETDATE())),

     -- Adega 2 - Humidade (%)
     (@SensorHumAdega2, 67.0, DATEADD(HOUR, -12, GETDATE())),
     (@SensorHumAdega2, 66.4, DATEADD(HOUR, -10, GETDATE())),
     (@SensorHumAdega2, 68.1, DATEADD(HOUR, -8, GETDATE())),
     (@SensorHumAdega2, 67.5, DATEADD(HOUR, -6, GETDATE())),
     (@SensorHumAdega2, 66.9, DATEADD(HOUR, -4, GETDATE())),
     (@SensorHumAdega2, 67.2, DATEADD(HOUR, -2, GETDATE())),

     -- Adega 2 - Luminosidade (lux)
     (@SensorLumAdega2, 130.0, DATEADD(HOUR, -12, GETDATE())),
     (@SensorLumAdega2, 120.0, DATEADD(HOUR, -10, GETDATE())),
     (@SensorLumAdega2, 105.0, DATEADD(HOUR, -8, GETDATE())),
     (@SensorLumAdega2, 85.0, DATEADD(HOUR, -6, GETDATE())),
     (@SensorLumAdega2, 70.0, DATEADD(HOUR, -4, GETDATE())),
     (@SensorLumAdega2, 55.0, DATEADD(HOUR, -2, GETDATE()));
