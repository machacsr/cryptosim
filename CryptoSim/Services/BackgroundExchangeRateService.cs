using CryptoSim.Dto;
using CryptoSim.Model;

namespace CryptoSim.Services;

public interface BackgroundExchangeRateService
{
    Task<Task> UpdateExchangeRateAsync(CancellationToken stoppingToken);
}