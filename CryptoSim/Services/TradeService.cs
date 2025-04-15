using CryptoSim.Dto;

namespace CryptoSim.Services;

public interface TradeService
{
    Task<CryptoTransactionDto> BuyCryptoAsync(int userId, CryptoTradeDto cryptoTradeDto);
    
    Task<CryptoTransactionDto> SellCryptoAsync(int userId, CryptoTradeDto cryptoTradeDto);
}