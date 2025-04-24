using System.ComponentModel.DataAnnotations;

namespace CryptoSim.Dto;

public class CreateCryptoDto
{
    [Required]
    public string Symbol { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public double InitialPrice { get; set; }
}