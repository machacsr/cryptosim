

-- --- Felhasználók ---
INSERT INTO Users (Id, Name, Email, Password)
VALUES
    (1, 'Alice', 'alice@example.com', '$2a$11$cQ1R7E0WVTgp/vJJmJ5n0.lnZ6Q3dlmqz3fro3pLGVWeRrD3ph.v6'),--password -> stringst
    (2, 'Bob', 'bob@example.com', '$2a$11$cQ1R7E0WVTgp/vJJmJ5n0.lnZ6Q3dlmqz3fro3pLGVWeRrD3ph.v6');--password -> stringst

-- --- Pénztárcák ---
INSERT INTO Wallets (UserId, Balance)
VALUES
    (1, 10000.00), -- Alice 10.000 HUF
    (2, 5000.00);  -- Bob 5.000 HUF

-- --- Kriptovaluták ---
INSERT INTO Cryptos (Name, Symbol)
VALUES
    ('Bitcoin', 'BTC' ),
    ('Ethereum', 'ETH' ),
    ('Solana', 'SOL' );

-- --- Listázások ---
INSERT INTO CryptoListings (CryptoId, Price, State)
VALUES
    (1, 29500.00, 'ACTIVE'),
    (2, 1750.00, 'ACTIVE'),
    (3, 95.00, 'ACTIVE');

-- --- Tranzakciók ---
INSERT INTO CryptoTransactions (Id, UserId, WalletId, CryptoListingId, TransactionType, Quantity, UnitPrice, TotalAmount, Timestamp)
VALUES
    ('f26c1475-55e3-4444-9fdd-df8f92def564', 1, 1, 1, 0, 1, 29500.00, 2950.00, CURRENT_TIMESTAMP),
    ('681dd320-32e2-4f78-9534-3dc6ca22780d', 1, 1, 2, 0, 2, 1750.00, 2625.00, CURRENT_TIMESTAMP),
    ('a2d53ab4-a719-4ca3-98fe-65e8add88266', 2, 2, 3, 0, 10.0, 95.00, 950.00, CURRENT_TIMESTAMP);
