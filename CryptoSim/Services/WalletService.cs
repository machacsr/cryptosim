using CryptoSim.Dto;

namespace CryptoSim.Services;

public interface WalletService
{
    Task<WalletDto> GetWalletAsync(int userId);

    Task<WalletDto> UpdateWalletAsync(int userId, WalletBalanceDto balanceDto);

    Task DeleteWalletAsync(int userId);
}