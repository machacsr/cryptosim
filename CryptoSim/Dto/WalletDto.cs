namespace CryptoSim.Dto;

public class WalletDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserDto User { get; set; }
    public double Balance { get; set; }
    public List<WalletCryptoItemDto> WalletCryptoItems { get; set; } = new();
    private List<CryptoTransactionDto> CryptoTransactions { get; set; } = new();
}

public class WalletBalanceDto
{
    public double Balance { get; set; }
}

public class WalletCryptoItemDto
{
    public int CryptoId { get; set; }
    public string Symbol { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
}