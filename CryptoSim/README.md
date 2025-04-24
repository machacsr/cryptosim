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


## First run

You have to run initdb.sql immediately after first run to fill database with inital data.
