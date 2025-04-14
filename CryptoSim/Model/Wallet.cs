using System.ComponentModel.DataAnnotations;

namespace CryptoSim.Model;

public class Wallet
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public double Balance { get; set; }

    public List<CryptoTransaction> CryptoTransactions { get; set; } = new();

}