using CryptoSim.Dto;
using CryptoSim.Model;

namespace CryptoSim.Utils;

public static class TransactionEvaluator
{
    /// <summary>
    /// Required Wallet deps: ["CryptoTransactions", "CryptoTransactions.CryptoListing", "CryptoTransactions.CryptoListing.Crypto"]
    /// </summary>
    public static List<WalletCryptoItemDto> Evaluate(Wallet wallet)
    {
        var currentWalletItems = new Dictionary<string, WalletCryptoItemDto>();
        foreach (var cryptoTransaction in wallet.CryptoTransactions)
        {
            var listing = cryptoTransaction.CryptoListing;
            var key = listing.Crypto.Symbol;

            var itemInWallet = currentWalletItems.GetValueOrDefault(key);

            if (cryptoTransaction.TransactionType == CryptoTransactionType.Buy && itemInWallet == null)
            {
                currentWalletItems.Add(listing.Crypto.Symbol, new WalletCryptoItemDto
                {
                    CryptoId = listing.Crypto.Id,
                    Symbol = listing.Crypto.Symbol,
                    Name = listing.Crypto.Name,
                    Quantity = cryptoTransaction.Quantity,
                });
            }
            else if (cryptoTransaction.TransactionType == CryptoTransactionType.Buy)
            {
                itemInWallet.Quantity += cryptoTransaction.Quantity;
            }
            else if (cryptoTransaction.TransactionType == CryptoTransactionType.Sell && itemInWallet == null)
            {
                // not exists case
            }
            else if (cryptoTransaction.TransactionType == CryptoTransactionType.Sell)
            {
                itemInWallet.Quantity -= cryptoTransaction.Quantity;
            }
        }

        return currentWalletItems.Values.ToList();
    }
    
    /// <summary>
    /// Required Wallet deps: ["CryptoTransactions", "CryptoTransactions.CryptoListing"]
    /// </summary>
    public static WalletCryptoItemDto EvaluateByCrypto(List<CryptoTransaction> cryptoTransactions, Crypto crypto)
    {
        var walletCryptoItem = new WalletCryptoItemDto()
        {
            CryptoId = crypto.Id,
            Symbol = crypto.Symbol,
            Name = crypto.Name,
            Quantity = 0
        };

        if (cryptoTransactions.Count < 0) return walletCryptoItem;
        
        // evaluate holding by transaction
        foreach (var cryptoTransaction in from cryptoTransaction in cryptoTransactions let listing = cryptoTransaction.CryptoListing select cryptoTransaction)
        {
            switch (cryptoTransaction.TransactionType)
            {
                case CryptoTransactionType.Buy:
                    walletCryptoItem.Quantity += cryptoTransaction.Quantity;
                    break;
                case CryptoTransactionType.Sell:
                    walletCryptoItem.Quantity -= cryptoTransaction.Quantity;
                    break;
            }
        }

        return walletCryptoItem;
    }
}