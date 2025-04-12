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
- CryptoTransactions

Crypto
- Id
- Name
- Symbol
- CryptoListings