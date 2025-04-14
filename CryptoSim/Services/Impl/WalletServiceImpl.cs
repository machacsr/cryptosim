using AutoMapper;
using CryptoSim.Dto;
using CryptoSim.Model;
using CryptoSim.Repository;

namespace CryptoSim.Services.Impl;

public class WalletServiceImpl(IUnitOfWork unitOfWork, IMapper mapper) : WalletService
{
    public async Task<WalletDto> GetWalletAsync(int userId)
    {
        var wallets = await unitOfWork.WalletRepository.GetAllAsync(wall => wall.UserId == userId, ["CryptoTransactions", "CryptoTransactions.CryptoListing", "CryptoTransactions.CryptoListing.Crypto"]).ContinueWith(it => it.Result.ToArray());

        if (wallets.Length == 0)
        {
            throw new Exception("There are no wallets associated with this user");
        }
        
        if (wallets.Length > 1)
        {
            throw new Exception("There is more than one wallet for the given userId");
        }

        var wallet = wallets.First();
        
        var currentWalletItems = new Dictionary<string, WalletCryptoItemDto>();
        foreach (var cryptoTransaction in wallet.CryptoTransactions)
        {
            var listing = cryptoTransaction.CryptoListing;
            var key = listing.Crypto.Symbol;

            var itemInWallet = currentWalletItems.GetValueOrDefault(key);
            
            if (cryptoTransaction.TransactionType == CryptoTransactionType.BUY)
            {
                if (itemInWallet == null)
                {
                    currentWalletItems.Add(listing.Crypto.Symbol, new WalletCryptoItemDto {
                            Symbol = listing.Crypto.Symbol,
                            Name = listing.Crypto.Name,
                            Quantity = cryptoTransaction.Quantity,
                        });
                }
                else
                {
                    itemInWallet.Quantity += cryptoTransaction.Quantity;
                }
            }
            
            if (cryptoTransaction.TransactionType == CryptoTransactionType.SELL)
            {
                if (itemInWallet == null)
                {
                    // not exists case
                }
                else
                {
                    itemInWallet.Quantity -= cryptoTransaction.Quantity;
                }
            }
        }

        var walletDto = mapper.Map<WalletDto>(wallet);
        walletDto.WalletCryptoItems = currentWalletItems.Values.ToList();
        return walletDto;
    }

    public async Task<WalletDto> UpdateWalletAsync(int userId, WalletBalanceDto balanceDto)
    {
        throw new NotImplementedException();
    }
}