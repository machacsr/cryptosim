using System.ComponentModel.DataAnnotations;

namespace CryptoSim.Model;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public Wallet? Wallet { get; set; }
    public List<CryptoTransaction> CryptoTransactions { get; set; } = new();
}