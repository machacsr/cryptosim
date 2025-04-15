using CryptoSim.Model;

namespace CryptoSim.Dto;

public class CryptoListingDto
{
    public int Id { get; set; }
    public int CryptoId { get; set; }
    //public CryptoDto Crypto { get; set; }

    public double Price { get; set; }
    public CryptoListingState State { get; set; }
    
    //public List<CryptoTransactionDto> CryptoTransactions { get; set; } = new();
}