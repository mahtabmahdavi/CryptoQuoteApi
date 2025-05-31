using System.Text.Json.Serialization;

namespace CryptoQuoteApi.Infrastructure.Models.CoinMarketCap;

public class CryptoData
{
    [JsonPropertyName("quote")]
    public Dictionary<string, QuoteInfo> Quote { get; set; } = new();
}
