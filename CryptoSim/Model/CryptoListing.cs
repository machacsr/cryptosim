using System.ComponentModel.DataAnnotations;

namespace CryptoSim.Model;

public class CryptoListing // mutable
{
    [Key]
    public int Id { get; set; }

    public int CryptoId { get; set; }
    public Crypto Crypto { get; set; }

    public double Price { get; set; }
    public CryptoListingState State { get; set; } // Only one active can be exits at time //FIXME
    public List<CryptoTransaction> CryptoTransactions { get; set; } = new();
}

public enum CryptoListingState
{
    Archived = 0,
    Active = 1, c
}