using AutoMapper;
using CryptoSim.Dto;
using CryptoSim.Model;
using CryptoSim.Repository;
using CryptoSim.Services.Exceptions;

namespace CryptoSim.Services.Impl;

public class WalletServiceImpl(IUnitOfWork unitOfWork, IMapper mapper) : WalletService
{
    public async Task<WalletDto> GetWalletAsync(int userId)
    {
        var wallet = await FindWalletByUserId(userId, ["CryptoTransactions", "CryptoTransactions.CryptoListing", "CryptoTransactions.CryptoListing.Crypto"]);
        
        var currentWalletItems = new Dictionary<string, WalletCryptoItemDto>();
        foreach (var cryptoTransaction in wallet.CryptoTransactions)
        {
            var listing = cryptoTransaction.CryptoListing;
            var key = listing.Crypto.Symbol;

            var itemInWallet = currentWalletItems.GetValueOrDefault(key);

            if (cryptoTransaction.TransactionType == CryptoTransactionType.BUY && itemInWallet == null)
            {
                currentWalletItems.Add(listing.Crypto.Symbol, new WalletCryptoItemDto
                {
                    Symbol = listing.Crypto.Symbol,
                    Name = listing.Crypto.Name,
                    Quantity = cryptoTransaction.Quantity,
                });
            }
            else if (cryptoTransaction.TransactionType == CryptoTransactionType.BUY)
            {
                itemInWallet.Quantity += cryptoTransaction.Quantity;
            }
            else if (cryptoTransaction.TransactionType == CryptoTransactionType.SELL && itemInWallet == null)
            {
                // not exists case
            }
            else if (cryptoTransaction.TransactionType == CryptoTransactionType.SELL)
            {
                itemInWallet.Quantity -= cryptoTransaction.Quantity;
            }
        }

        var walletDto = mapper.Map<WalletDto>(wallet);
        walletDto.WalletCryptoItems = currentWalletItems.Values.ToList();
        return walletDto;
    }

    public async Task<WalletDto> UpdateWalletAsync(int userId, WalletBalanceDto balanceDto)
    {
        var wallet = await FindWalletByUserId(userId);

        if (balanceDto == null)
        {
            throw new BadRequestException("Validation error", "BalanceDto cannot be null.");
        }
        
        wallet.Balance = balanceDto.Balance;
        unitOfWork.WalletRepository.Update(wallet);
        await unitOfWork.SaveAsync();

        return mapper.Map<WalletDto>(wallet);
    }

    public async Task DeleteWalletAsync(int userId)
    {
        var wallet = await FindWalletByUserId(userId);
        unitOfWork.WalletRepository.Delete(wallet);
        await unitOfWork.SaveAsync();
    }

    private async Task<Wallet> FindWalletByUserId(int userId, string[]? includeProperties = null)
    {
        var wallets = await unitOfWork.WalletRepository.GetAllAsync(wall => wall.UserId == userId, includeProperties).ContinueWith(it => it.Result.ToArray());

        if (wallets.Length == 0)
        {
            throw new Exception("There are no wallets associated with this user");
        }
        
        if (wallets.Length > 1)
        {
            throw new Exception("There is more than one wallet for the given userId");
        }

        return wallets.First();
    }
}