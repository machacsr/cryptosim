using CryptoSim.Dto;

namespace CryptoSim.Services;

public interface PortfolioService
{
    Task<PortfolioDto> GetPortfolio(int userId);
}