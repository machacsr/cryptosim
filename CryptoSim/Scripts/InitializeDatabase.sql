-- --- Felhasználók ---
INSERT INTO Users (Id, Name, Email, Password)
VALUES
    (1, 'Alice', 'alice@example.com', '$2a$11$cQ1R7E0WVTgp/vJJmJ5n0.lnZ6Q3dlmqz3fro3pLGVWeRrD3ph.v6'),--password -> stringst
    (2, 'Bob', 'bob@example.com', '$2a$11$cQ1R7E0WVTgp/vJJmJ5n0.lnZ6Q3dlmqz3fro3pLGVWeRrD3ph.v6');--password -> stringst

-- --- Pénztárcák ---
INSERT INTO Wallets (UserId, Balance)
VALUES
    (1, 100000.00), -- Alice 100.000 HUF
    (2, 50000.00);  -- Bob 50.000 HUF

-- --- Kriptovaluták ---
INSERT INTO Cryptos (Id, Name, Symbol)
VALUES
    (1, 'Bitcoin', 'BTC'),
    (2, 'Ethereum', 'ETH'),
    (3, 'Solana', 'SOL'),
    (4, 'Cardano', 'ADA'),
    (5, 'Polkadot', 'DOT'),
    (6, 'Ripple', 'XRP'),
    (7, 'Litecoin', 'LTC'),
    (8, 'Chainlink', 'LINK'),
    (9, 'Stellar', 'XLM'),
    (10, 'Uniswap', 'UNI'),
    (11, 'Avalanche', 'AVAX'),
    (12, 'VeChain', 'VET'),
    (13, 'Algorand', 'ALGO'),
    (14, 'Cosmos', 'ATOM'),
    (15, 'Filecoin', 'FIL');

-- --- Listázások ---
INSERT INTO CryptoListings (CryptoId, Price, State)
VALUES
    (1, 29500.00, 'Active'),
    (2, 1750.00, 'Active'),
    (3, 95.00, 'Active'),
    (4, 0.35, 'Active'),
    (5, 6.20, 'Active'),
    (6, 0.50, 'Active'),
    (7, 85.00, 'Active'),
    (8, 7.80, 'Active'),
    (9, 0.11, 'Active'),
    (10, 5.40, 'Active'),
    (11, 18.30, 'Active'),
    (12, 0.025, 'Active'),
    (13, 0.16, 'Active'),
    (14, 8.90, 'Active'),
    (15, 4.75, 'Active');

-- --- Tranzakciók ---
INSERT INTO CryptoTransactions (Id, UserId, WalletId, CryptoListingId, CryptoId, TransactionType, Quantity, UnitPrice, TotalAmount, Timestamp)
VALUES
    ('f26c1475-55e3-4444-9fdd-df8f92def564', 1, 1, 1, 1,'Buy', 1, 29500.00, 2950.00, CURRENT_TIMESTAMP),
    ('681dd320-32e2-4f78-9534-3dc6ca22780d', 1, 1, 2, 2, 'Buy', 2, 1750.00, 2625.00, CURRENT_TIMESTAMP),
    ('a2d53ab4-a719-4ca3-98fe-65e8add88266', 2, 2, 3, 3, 'Buy', 10.0, 95.00, 950.00, CURRENT_TIMESTAMP);
