using CryptoSim.Dto;

namespace CryptoSim.Services;

public interface CryptoService
{
    Task<List<CryptoListingMarketplaceDto>> GetAllLatestCryptoListingAsync();

    Task<CryptoDto> GetCryptoAsync(int cryptoId);

    Task<CryptoDto> CreateCryptoAsync(CreateCryptoDto cryptoDto);

    Task DeleteCryptoAsync(int cryptoId);
}