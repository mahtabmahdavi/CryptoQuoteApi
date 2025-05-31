using System.Text.Json.Serialization;

namespace CryptoQuoteApi.Infrastructure.Models.CoinMarketCap;

public class QuoteInfo
{
    [JsonPropertyName("price")]
    public decimal? Price { get; set; }
}
