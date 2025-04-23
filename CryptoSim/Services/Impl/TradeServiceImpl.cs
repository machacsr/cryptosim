using AutoMapper;
using CryptoSim.Dto;
using CryptoSim.Model;
using CryptoSim.Repository;
using CryptoSim.Services.Exceptions;
using CryptoSim.Utils;

namespace CryptoSim.Services.Impl;

public class TradeServiceImpl(IUnitOfWork unitOfWork, IMapper mapper, AppDbContext dbContext) : TradeService
{
    public async Task<CryptoTransactionDto> BuyCryptoAsync(int userId, CryptoTradeDto cryptoTradeDto)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId, ["Wallet"], ["CryptoTransactions"]);
        if (user == null)
        {
            throw new BadRequestException("Validation error", "User not found");
        }
        
        if (user.Wallet == null)
        {
            throw new BadRequestException("Validation error", "Wallet not found");
        }
        
        if (await unitOfWork.CryptoRepository.GetByIdAsync(cryptoTradeDto.CryptoId) == null)
        {
            throw new BadRequestException("Validation error", "Crypto not found");
        }
        
        // get actual price of selected crypto
        var latestCryptoListing = (await unitOfWork.CryptoListingRepository.GetAllAsync(listing => listing.CryptoId == cryptoTradeDto.CryptoId && listing.State == CryptoListingState.Active)).First();
        
        var purchasePrice = latestCryptoListing.Price * cryptoTradeDto.Quantity;

        if (user.Wallet.Balance < purchasePrice)
        {
            throw new BadRequestException("Validation error", "Not enough balance");
        }
        
        // begin transaction
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

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
            CryptoId = latestCryptoListing.CryptoId,
            TransactionType = CryptoTransactionType.Buy,
            Quantity = cryptoTradeDto.Quantity,
            UnitPrice = latestCryptoListing.Price,
            TotalAmount = purchasePrice,
            Timestamp = DateTime.Now,
        };

        dbContext.CryptoTransactions.Add(cryptoTransaction); // 1 insert transaction
        user.Wallet.CryptoTransactions.Add(cryptoTransaction); // 2 add transaction details to wallet
        dbContext.Users.Update(user); // 3 update user
        
        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        
        return mapper.Map<CryptoTransactionDto>(cryptoTransaction);
    }

    public async Task<CryptoTransactionDto> SellCryptoAsync(int userId, CryptoTradeDto cryptoTradeDto)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId, ["Wallet"], ["CryptoTransactions"]);
        if (user == null)
        {
            throw new BadRequestException("Validation error", "User not found");
        }
        
        if (user.Wallet == null)
        {
            throw new BadRequestException("Validation error", "Wallet not found");
        }

        var crypto = await unitOfWork.CryptoRepository.GetByIdAsync(cryptoTradeDto.CryptoId);
        if (crypto == null)
        {
            throw new BadRequestException("Validation error", "Crypto not found");
        }
        
        // Check holdings
        var holdedCryptos = TransactionEvaluator.EvaluateByCrypto(user.CryptoTransactions.Where(transaction => transaction.CryptoId == crypto.Id).ToList(), crypto);
        if (holdedCryptos == null || holdedCryptos.Quantity < cryptoTradeDto.Quantity)
        {
            throw new BadRequestException("Validation error", "Not enough balance or don't have crypto");
        }
        
        // get actual price of owned crypto
        var latestCryptoListing = (await unitOfWork.CryptoListingRepository.GetAllAsync(listing => listing.CryptoId == cryptoTradeDto.CryptoId && listing.State == CryptoListingState.Active)).First();
        
        var sellingPrice = latestCryptoListing.Price * cryptoTradeDto.Quantity;

        // begin transaction
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        
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
            CryptoId = latestCryptoListing.CryptoId,
            TransactionType = CryptoTransactionType.Sell,
            Quantity = cryptoTradeDto.Quantity,
            UnitPrice = latestCryptoListing.Price,
            TotalAmount = sellingPrice,
            Timestamp = DateTime.Now
        };
        dbContext.CryptoTransactions.Add(cryptoTransaction); // Insert to db
        user.Wallet.CryptoTransactions.Add(cryptoTransaction); //Add to wallet

        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        
        return mapper.Map<CryptoTransactionDto>(cryptoTransaction);
    }
}