/* ============================================================
   TABELA: ADEGA (duas adegas somente)
   ============================================================ */

INSERT INTO [ISI].[dbo].[Adega] (Nome, Localizacao, Capacidade)
VALUES 
    ('Adega do Vale', 'Douro', 50000),      -- ID 1
    ('Quinta da Serra', 'Dão', 30000);      -- ID 2



/* ============================================================
   TABELA: VINHOS (12 vinhos)
   ============================================================ */

INSERT INTO [ISI].[dbo].[Vinhos] 
    (Nome, Produtor, Ano, Tipo, Descricao, ImagemUrl, Preco)
VALUES
    -- Adega 1 ----------------------------------------------------
    ('Reserva do Vale', 'Adega do Vale', 2018, 'Tinto', 
     'Vinho tinto encorpado e elegante, com aromas intensos a frutos vermelhos maduros, notas subtis de especiarias e taninos bem integrados. Final persistente e equilibrado, ideal para acompanhar carnes vermelhas e pratos tradicionais.',
     'https://www.casaamerico.pt/wp-content/uploads/2024/10/Quinta-do-Vale-reserva-tinto.jpg', 18.50),      -- ID 1

    ('Rosé do Vale', 'Adega do Vale', 2021, 'Rosé', 
     'Rosé fresco e aromático, com notas delicadas de frutos vermelhos e florais. Leve, vibrante e muito fácil de beber, perfeito para dias quentes, aperitivos ou refeições leves.',
     'https://www.almadeportugal.com/cdn/shop/files/qta-vale-tinto-reserva.png?v=1712760543', 9.90),       -- ID 2

    ('Branco do Vale Reserva', 'Adega do Vale', 2022, 'Branco', 
     'Branco muito aromático e expressivo, com notas de fruta branca madura, citrinos e ligeiro toque mineral. Na boca é fresco, equilibrado e persistente, ideal para peixe, marisco e saladas.',
     'https://cdnx.jumpseller.com/selectmoment/image/46760355/vllvr.jpg?1711042103', 14.30),               -- ID 3

    ('Superior do Vale', 'Adega do Vale', 2017, 'Tinto', 
     'Vinho tinto de grande estrutura e complexidade, com notas de fruta preta madura, madeira bem integrada e especiarias. Taninos firmes e final longo, ideal para pratos intensos e momentos especiais.',
     'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRn8_-iYmW1K8YmyvFpXea8-OJ4zk-jMOMkQQ&s', 25.50), -- ID 4

    ('Vale Late Harvest', 'Adega do Vale', 2021, 'Doce', 
     'Vinho doce elegante, com doçura equilibrada por uma acidez fresca. Apresenta aromas de fruta madura, mel e notas florais. Excelente para sobremesas, queijos ou como vinho de contemplação.',
     'https://www.portugalvineyards.com/124018-large_default/les-a-les-late-harvest-2021.jpg', 19.90),     -- ID 5

    ('Espumante Vale Bruto', 'Adega do Vale', 2020, 'Espumante', 
     'Espumante fresco e elegante, com bolha fina e persistente. Aromas florais e frutados, boca equilibrada e refrescante. Ideal para celebrações, aperitivos e momentos especiais.',
     'https://bonsbagos.com/products/quinta-vale-daldeia-espumante-bruto-nature-2020?srsltid=AfmBOorLBRoCheLl6P7Lk2AWtmUbWMEdM1owpHpKGKTgCOIVk1LZMBP3', 11.80), -- ID 6


    -- Adega 2 ----------------------------------------------------
    ('Branco da Serra', 'Quinta da Serra', 2020, 'Branco', 
     'Branco fresco e mineral, com aromas cítricos e notas florais subtis. Na boca apresenta boa acidez e equilíbrio, sendo perfeito para pratos leves, peixe grelhado e marisco.',
     'https://garrafeiracopocheio.pt/908-home_default/quinta-serra-doura-reserva-branco-2017.jpg', 12.00),  -- ID 7

    ('Tinto da Serra', 'Quinta da Serra', 2019, 'Tinto', 
     'Vinho tinto harmonioso, com aromas a frutos pretos maduros e ligeiro toque especiado. Taninos macios e final suave, ideal para consumo diário e refeições tradicionais.',
     'https://www.portugalvineyards.com/95670/quinta-serra-doura-reserve-white-2017.jpg', 15.20),          -- ID 8

    ('Serra Reserva Especial', 'Quinta da Serra', 2016, 'Tinto', 
     'Tinto complexo e sofisticado, com notas profundas de fruta madura, especiarias e madeira bem integrada. Taninos sedosos e final longo e elegante. Ideal para ocasiões especiais.',
     'https://garrafeiracopocheio.pt/905-home_default/quinta-serra-doura-reserva-tinto-2016.jpg', 27.00),  -- ID 9

    ('Serra Rosé Premium', 'Quinta da Serra', 2022, 'Rosé', 
     'Rosé premium leve e aromático, com notas de frutos silvestres e excelente frescura. Elegante e equilibrado, perfeito para acompanhar pratos leves ou desfrutar a solo.',
     'https://www.portugalvineyards.com/95673-home_default/quinta-serra-doura-reserve-rose-2017.jpg', 10.50), -- ID 10

    ('Serra Colheita Selecionada', 'Quinta da Serra', 2021, 'Branco', 
     'Branco expressivo com aromas cítricos e notas minerais elegantes. Boca fresca, equilibrada e persistente, ideal para peixe, marisco e cozinha mediterrânica.',
     'https://www.vinalda.pt/media/productimages/172.009_serramaecolheitabranco.png', 13.40),              -- ID 11

    ('Serra Licoroso', 'Quinta da Serra', 2019, 'Licoroso', 
     'Vinho licoroso doce e envolvente, com aromas intensos e final longo e persistente. Excelente para acompanhar sobremesas, queijos ou para momentos de degustação tranquila.',
     'https://cdnx.jumpseller.com/gota-a-gota-wine-house/image/62871933/resize/640/640?1745516408', 18.00); -- ID 12



/* ============================================================
   TABELA: STOCK (ligação Vinhos <-> Adegas)
   ============================================================ */

INSERT INTO [ISI].[dbo].[Stock] (VinhosId, AdegaId)
VALUES
    -- Adega 1
    (1, 1),
    (2, 1),
    (3, 1),
    (4, 1),
    (5, 1),
    (6, 1),

    -- Adega 2
    (7, 2),
    (8, 2),
    (9, 2),
    (10, 2),
    (11, 2),
    (12, 2);