using CryptoSim.Dto;

namespace CryptoSim.Services;

public interface CryptoService
{
    Task<CryptoDto> GetAllCryptoAsync();

    Task<CryptoDto> GetCryptoAsync(int cryptoId);

    Task<CryptoDto> CreateCryptoAsync(CryptoDto cryptoDto);

    Task DeleteCryptoAsync(int cryptoId);
}