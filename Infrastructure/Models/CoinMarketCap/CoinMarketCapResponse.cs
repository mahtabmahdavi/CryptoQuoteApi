using System.Text.Json.Serialization;

namespace CryptoQuoteApi.Infrastructure.Models.CoinMarketCap;

public class CoinMarketCapResponse
{
    [JsonPropertyName("data")]
    public Dictionary<string, CryptoData> Data { get; set; } = new();
}
