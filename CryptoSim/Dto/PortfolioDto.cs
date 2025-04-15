using System.ComponentModel.DataAnnotations;

namespace CryptoSim.Dto;

public class PortfolioDto
{
    public int UserId { get; set; }
    public List<PortfolioItemDto> PortfolioItems { get; set; } = new();
    public double TotalValue { get; set; }
}


public class PortfolioItemDto : WalletCryptoItemDto
{
    public double CurrentPrice { get; set; }
    public double SubTotalValue { get; set; }
}


public class CryptoTradeDto
{
    [Required]
    public int CryptoId { get; set; }
    
    [Required]
    public int Quantity { get; set; }
}