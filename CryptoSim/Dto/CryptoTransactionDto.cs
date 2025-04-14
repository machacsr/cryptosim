using CryptoSim.Model;

namespace CryptoSim.Dto;

public class CryptoTransactionDto
{
    public Guid Id { get; set; }
    public int UserId { get; set; }
    public int WalletId { get; set; }
    public int CryptoListingId { get; set; }
    public CryptoListingDto CryptoListingDto { get; set; }
    public CryptoTransactionType TransactionType { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double TotalAmount { get; set; }
    public DateTime Timestamp { get; set; }
}