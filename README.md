# Crypto Trading Simulator

## Used technologies
- .NET 9
- C#

## System Design - Models

User 
- Id 
- Name 
- Email 
- Password
- Wallet
- CryptoTransactions

Wallet
- Id
- UserId
- Balance (Decimal)
- CryptoTransactions

CryptoTransaction
- Id
- UserId
- WalletId
- CryptoListingId
- CryptoId // only for easier computation
- TransactionType - Buy, Sell
- Quantity
- UnitPrice
- TotalAmount (Quanity * PurchasePrice)
- Timestamp


CryptoListing
- Id
- CryptoId
- Price
- CryptoListingState // Active, Archived // Only one active can be exits at time
- ArchivedAt // timestamp
- CryptoTransactions

Crypto
- Id
- Name
- Symbol
- CryptoListings


## Login

### Alice
- Username: alice@example.com
- Password: stringst
- UserId: 1
- WalletId: 1
- Balance: 50.000 Magyar Potyánka

### Bob
- Username: bob@example.com
- Password: stringst
- UserId: 2
- WalletId: 2
- Balance: 100.000 Magyar Potyánka


## Available Cryptos

| ID | Symbol | Name      |
|----|--------|-----------|
| 1  | BTC    | Bitcoin   |
| 2  | ETH    | Ethereum  |
| 3  | SOL    | Solana    |
| 4  | ADA    | Cardano   |
| 5  | DOT    | Polkadot  |
| 6  | XRP    | Ripple    |
| 7  | LTC    | Litecoin  |
| 8  | LINK   | Chainlink |
| 9  | XLM    | Stellar   |
| 10 | UNI    | Uniswap   |
| 11 | AVAX   | Avalanche |
| 12 | VET    | VeChain   |
| 13 | ALGO   | Algorand  |
| 14 | ATOM   | Cosmos    |
| 15 | FIL    | Filecoin  |
