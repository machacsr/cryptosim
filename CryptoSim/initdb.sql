-- --- Felhasználók ---
INSERT INTO User (Name, Email, Password)
VALUES
    ('Alice', 'alice@example.com', 'hashed-password-1'),
    ('Bob', 'bob@example.com', 'hashed-password-2');

-- --- Pénztárcák ---
INSERT INTO Wallet (UserId, Balance)
VALUES
    (1, 10000.00), -- Alice 10.000 HUF
    (2, 5000.00);  -- Bob 5.000 HUF

-- --- Kriptovaluták ---
INSERT INTO Crypto (Name, Symbol, Price, Quantity)
VALUES
    ('Bitcoin', 'BTC', 30000.00, 100),
    ('Ethereum', 'ETH', 1800.00, 500),
    ('Solana', 'SOL', 100.00, 1000);

-- --- Listázások ---
INSERT INTO CryptoListing (CryptoId, InitialPrice, InitialAmount, OfferValidTo)
VALUES
    (1, 29500.00, 5, DATETIME('now', '+7 days')),
    (2, 1750.00, 20, DATETIME('now', '+7 days')),
    (3, 95.00, 100, DATETIME('now', '+3 days'));

-- --- Tranzakciók ---
INSERT INTO CryptoTransaction (UserId, WalletId, CryptoListingId, TransactionType, Quantity, UnitPrice, TotalAmount, Timestamp)
VALUES
    (1, 1, 1, 'Buy', 0.1, 29500.00, 2950.00, CURRENT_TIMESTAMP),
    (1, 1, 2, 'Buy', 1.5, 1750.00, 2625.00, CURRENT_TIMESTAMP),
    (2, 2, 3, 'Buy', 10.0, 95.00, 950.00, CURRENT_TIMESTAMP);
