namespace CryptoQuoteApi.Application.Dtos;

public class CryptoQuoteResponse
{
    public string Symbol { get; set; } = string.Empty;
    public Dictionary<string, decimal> Quotes { get; set; } = new();
    public DateTime LastUpdated { get; set; }
}
