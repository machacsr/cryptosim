using System.ComponentModel.DataAnnotations;

namespace CryptoSim.Dto;

public class CryptoDto
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    public string Name { get; set; }
    
    public  List<CryptoListingDto> CryptoListings { get; set; } = new();
}

public class CreateCryptoDto
{
    [Required]
    public string Symbol { get; set; }
    
    
    [Required]
    public string Name { get; set; }
    
    
    [Required]
    public double InitialPrice { get; set; }
}