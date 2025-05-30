using CryptoQuoteApi.Application.Settings;
using CryptoQuoteApi.Infrastructure.Models.ExchangeRates;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CryptoQuoteApi.Infrastructure.Services;

public class ExchangeRatesService
{
    private readonly HttpClient _httpClient;
    private readonly ExchangeRatesSettings _settings;
    private readonly ILogger<ExchangeRatesService> _logger;

    private static readonly string[] TargetCurrencies = { "USD", "EUR", "BRL", "GBP", "AUD" };
    public ExchangeRatesService(HttpClient httpClient,
        IOptions<ExternalApiSettings> settings,
        ILogger<ExchangeRatesService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value.ExchangeRates;
        _logger = logger;

        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
    }

    public async Task<Dictionary<string, decimal>> GetExchangeRatesFromEurAsync()
    {
        try
        {
            var symbols = string.Join(",", TargetCurrencies);
            var response = await _httpClient.GetAsync($"latest?access_key={_settings.ApiKey}&base=EUR&symbols={symbols}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ExchangeRatesResponse>(content);

            if (result?.Rates is null)
            {
                throw new Exception("Failed to get exchange rates");
            }

            return result.Rates;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting exchange rates from ExchangeRates API");
            throw;
        }
    }
}
