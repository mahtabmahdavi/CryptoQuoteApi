using CryptoQuoteApi.Application.Exceptions;
using CryptoQuoteApi.Application.Interfaces;
using CryptoQuoteApi.Application.Settings;
using CryptoQuoteApi.Infrastructure.Models.ExchangeRates;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CryptoQuoteApi.Infrastructure.Services;

public class ExchangeRatesService : IExchangeRatesService
{
    private readonly HttpClient _httpClient;
    private readonly ExchangeRatesSettings _settings;

    private static readonly string[] TargetCurrencies = { "USD", "EUR", "BRL", "GBP", "AUD" };
    public ExchangeRatesService(HttpClient httpClient,
        IOptions<ExternalApiSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value.ExchangeRates;

        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
    }

    public async Task<Dictionary<string, decimal>> GetExchangeRatesFromEurAsync()
    {
        var symbols = string.Join(",", TargetCurrencies);
        var response = await _httpClient.GetAsync($"latest?access_key={_settings.ApiKey}&base=EUR&symbols={symbols}");
        if (!response.IsSuccessStatusCode)
        {
            throw new ExternalApiException("ExchangeRates API call failed.");
        }

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ExchangeRatesResponse>(content);

        if (result?.Rates is null)
        {
            throw new NotFoundException($"Failed to parse exchange rates.");
        }

        return result.Rates;
    }
}
