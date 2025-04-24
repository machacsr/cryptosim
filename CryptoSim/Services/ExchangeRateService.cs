using CryptoSim.Dto;
using CryptoSim.Model;

namespace CryptoSim.Services;

public interface ExchangeRateService
{
    Task<Task> UpdateExchangeRateAsync(CancellationToken stoppingToken);
    
    Task<CryptoListingDto> CreateCryptoListingAsync(CreateCryptoListingDto cryptoListingDto);
    
    Task<List<CryptoListingDto>> GetListingsForCrypto(int cryptoId);
}