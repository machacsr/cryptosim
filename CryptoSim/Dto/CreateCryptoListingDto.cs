using System.ComponentModel.DataAnnotations;

namespace CryptoSim.Dto;

public class CreateCryptoListingDto
{
    [Required]
    public int CryptoId { get; set; }
    
    [Required]
    public double NewPrice { get; set; }
}