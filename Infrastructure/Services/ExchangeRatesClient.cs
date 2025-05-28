using CryptoQuoteApi.Infrastructure.Models.ExchangeRates;
using System.Text.Json;

namespace CryptoQuoteApi.Infrastructure.Services;

public class ExchangeRatesClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<ExchangeRatesClient> _logger;

    private static readonly string[] TargetCurrencies = { "USD", "BRL", "GBP", "AUD" };

    public ExchangeRatesClient(IHttpClientFactory httpClientFactory, IConfiguration config, ILogger<ExchangeRatesClient> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ExchangeRates");
        _apiKey = config["ExchangeRates:ApiKey"];
        _logger = logger;
    }

    public async Task<Dictionary<string, decimal>> GetRatesFromEurAsync()
    {
        try
        {
            var symbols = string.Join(",", TargetCurrencies);
            var response = await _httpClient.GetAsync($"latest?access_key={_apiKey}&base=EUR&symbols={symbols}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ExchangeRatesResponse>(content);

            return result?.Rates ?? new Dictionary<string, decimal>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching exchange rates");
            return new Dictionary<string, decimal>();
        }
    }
}
