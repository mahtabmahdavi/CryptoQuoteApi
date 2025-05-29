using CryptoQuoteApi.Infrastructure.Models.CoinMarketCap;
using System.Text.Json;

namespace CryptoQuoteApi.Infrastructure.Services;

public class CoinMarketCapClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CoinMarketCapClient> _logger;

    public CoinMarketCapClient(IHttpClientFactory httpClientFactory, ILogger<CoinMarketCapClient> logger)
    {
        _httpClient = httpClientFactory.CreateClient("CoinMarketCap");
        _logger = logger;
    }

    public async Task<decimal?> GetPriceInEurAsync(string symbol)
    {
        try
        {
            var response = await _httpClient.GetAsync($"cryptocurrency/quotes/latest?symbol={symbol}&convert=EUR");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<CoinMarketCapResponse>(content);

            if (result?.Data != null && result.Data.TryGetValue(symbol.ToUpper(), out var data))
            {
                return data.Quote["EUR"].Price;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching price from CoinMarketCap for {symbol}");
            return null;
        }
    }
}
