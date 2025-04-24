namespace CryptoSim.Dto;

public class ProfitDto
{
    public double TotalDelta { get; set; }
}

public class ProfitDetailsDto : ProfitDto
{
    public List<CryptoProfitDetail> CryptoDeltas { get; set; } = new();
}

public class CryptoProfitDetail
{
    public int CryptoId { get; set; }     
    public string Symbol { get; set; }         
    public string Name { get; set; }         
    public int Quantity { get; set; }      
    public double MarketPrice { get; set; }  
    public double Delta { get; set; }  
}