using System.Text.Json.Serialization;

namespace CryptoQuoteApi.Infrastructure.Models.ExchangeRates;

public class ExchangeRatesResponse
{
    [JsonPropertyName("rates")]
    public Dictionary<string, decimal> Rates { get; set; } = new();
}
