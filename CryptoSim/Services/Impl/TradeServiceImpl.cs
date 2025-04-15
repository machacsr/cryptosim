using AutoMapper;
using CryptoSim.Dto;
using CryptoSim.Model;
using CryptoSim.Repository;
using CryptoSim.Services.Exceptions;

namespace CryptoSim.Services.Impl;

public class TradeServiceImpl(IUnitOfWork unitOfWork, IMapper mapper, AppDbContext dbContext) : TradeService
{
    public async Task<CryptoTransactionDto> BuyCryptoAsync(int userId, CryptoTradeDto cryptoTradeDto)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId, ["Wallet", "CryptoTransactions"]);
        if (user == null)
        {
            throw new BadRequestException("Validation error", "User not found");
        }
        
        // get actual price of selected crypto
        var latestCryptoListing = (await unitOfWork.CryptoListingRepository.GetAllAsync(listing => listing.CryptoId == cryptoTradeDto.CryptoId && listing.State == CryptoListingState.ACTIVE)).First();
        
        var purchasePrice = latestCryptoListing.Price * cryptoTradeDto.Quantity;

        if (user.Wallet.Balance < purchasePrice)
        {
            throw new BadRequestException("Validation error", "Not enough balance");
        }
        
        // begin transaction
        var transaction = await dbContext.Database.BeginTransactionAsync();
        
        user.Wallet.Balance -= purchasePrice;
        var cryptoTransaction = new CryptoTransaction()
        {
            Id = Guid.NewGuid(),
            User = user,
            UserId = user.Id,
            Wallet = user.Wallet,
            WalletId = user.Wallet.Id,
            CryptoListing = latestCryptoListing,
            CryptoListingId = latestCryptoListing.Id,
            TransactionType = CryptoTransactionType.BUY,
            Quantity = cryptoTradeDto.Quantity,
            UnitPrice = latestCryptoListing.Price,
            TotalAmount = purchasePrice,
            Timestamp = new DateTime(),
        };
        user.Wallet.CryptoTransactions.Add(cryptoTransaction);

        await transaction.CommitAsync();
        
        return mapper.Map<CryptoTransactionDto>(cryptoTransaction);
    }

    public async Task<CryptoTransactionDto> SellCryptoAsync(int userId, CryptoTradeDto cryptoTradeDto)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId, ["Wallet", "CryptoTransactions"]);
        if (user == null)
        {
            throw new BadRequestException("Validation error", "User not found");
        }
        
        // get actual price of owned crypto
        var latestCryptoListing = (await unitOfWork.CryptoListingRepository.GetAllAsync(listing => listing.CryptoId == cryptoTradeDto.CryptoId && listing.State == CryptoListingState.ACTIVE)).First();
        
        var sellingPrice = latestCryptoListing.Price * cryptoTradeDto.Quantity;

        // begin transaction
        var transaction = await dbContext.Database.BeginTransactionAsync();
        
        user.Wallet.Balance += sellingPrice;
        var cryptoTransaction = new CryptoTransaction()
        {
            Id = Guid.NewGuid(),
            User = user,
            UserId = user.Id,
            Wallet = user.Wallet,
            WalletId = user.Wallet.Id,
            CryptoListing = latestCryptoListing,
            CryptoListingId = latestCryptoListing.Id,
            TransactionType = CryptoTransactionType.SELL,
            Quantity = cryptoTradeDto.Quantity,
            UnitPrice = latestCryptoListing.Price,
            TotalAmount = sellingPrice,
            Timestamp = new DateTime(),
        };
        user.Wallet.CryptoTransactions.Add(cryptoTransaction);

        await transaction.CommitAsync();
        
        return mapper.Map<CryptoTransactionDto>(cryptoTransaction);
    }
}