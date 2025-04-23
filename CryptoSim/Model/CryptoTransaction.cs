using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CryptoSim.Model;

public class CryptoTransaction
{
    [Key] public Guid Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int WalletId { get; set; }
    public Wallet? Wallet { get; set; }
    public int CryptoListingId { get; set; }
    public CryptoListing CryptoListing { get; set; } // Connected to actual available item on the market
    
    public int CryptoId { get; set; }
    
    [Column(TypeName = "nvarchar(4)")]
    public CryptoTransactionType TransactionType { get; set; } // Buy, sell //FIXME

    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double TotalAmount { get; set; }
    public DateTime Timestamp { get; set; }
}

public enum CryptoTransactionType
{
    [EnumMember(Value = "Sell")] 
    Sell,
    
    [EnumMember(Value = "Buy")] 
    Buy,
}