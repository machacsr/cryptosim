using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CryptoSim.Model;

public class CryptoListing // mutable
{
    [Key]
    public int Id { get; set; }

    public int CryptoId { get; set; }
    public Crypto Crypto { get; set; }

    public double Price { get; set; }
    
    [Column(TypeName = "nvarchar(8)")]
    public CryptoListingState State { get; set; } // Only one active can be exits at time //FIXME
    public DateTime? ArchivedAt { get; set; }
    public List<CryptoTransaction> CryptoTransactions { get; set; } = new();
}

public enum CryptoListingState
{
    [EnumMember(Value = "Archived")] 
    Archived,
    
    [EnumMember(Value = "Active")] 
    Active,
}