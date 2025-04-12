using System.ComponentModel.DataAnnotations;

namespace CryptoSim.Model;

public class Crypto
{
    [Key]
    public int Id { get; set; }
    public string Symbol { get; set; }
    public string Name { get; set; }

    public  List<CryptoListing> CryptoListings { get; set; } = new(); // You can buy the latest listing
}