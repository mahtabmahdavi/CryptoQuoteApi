using CryptoQuoteApi.Application.Settings;
using CryptoQuoteApi.Infrastructure.Models.CoinMarketCap;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CryptoQuoteApi.Infrastructure.Services;

public class CoinMarketCapService
{
    private readonly HttpClient _httpClient;
    private readonly CoinMarketCapSettings _settings;
    private readonly ILogger<CoinMarketCapService> _logger;

    public CoinMarketCapService(
        HttpClient httpClient,
        IOptions<ExternalApiSettings> settings,
        ILogger<CoinMarketCapService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value.CoinMarketCap;
        _logger = logger;

        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", _settings.ApiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<decimal> GetCryptoPriceInEurAsync(string symbol)
    {
        try
        {
            var response = await _httpClient.GetAsync($"cryptocurrency/quotes/latest?symbol={symbol}&convert=EUR");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CoinMarketCapResponse>(content);

            if (result?.Data is null || !result.Data.ContainsKey(symbol.ToUpper()))
            {
                throw new Exception($"No data found for symbol: {symbol}");
            }

            return result.Data[symbol].Quote["EUR"].Price;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting price from CoinMarketCap for symbol: {symbol}");
            throw;
        }
    }
}
