using System.Text.Json.Serialization;
using CryptoSim.Model;

namespace CryptoSim.Dto;

public class CryptoListingDto
{
    public int Id { get; set; }
    public int CryptoId { get; set; }
    //public CryptoDto Crypto { get; set; }

    public double Price { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter<CryptoListingState>))]
    public CryptoListingState State { get; set; }
    
    public DateTime? ArchivedAt { get; set; }
    
    //public List<CryptoTransactionDto> CryptoTransactions { get; set; } = new();
}

public class CryptoListingMarketplaceDto
{
    public string CryptoId { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public double Price { get; set; }
}