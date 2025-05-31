using CryptoQuoteApi.Application.Exceptions;
using CryptoQuoteApi.Application.Interfaces;
using CryptoQuoteApi.Application.Settings;
using CryptoQuoteApi.Infrastructure.Models.CoinMarketCap;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CryptoQuoteApi.Infrastructure.Services;

public class CoinMarketCapService : ICoinMarketCapService
{
    private readonly HttpClient _httpClient;
    private readonly CoinMarketCapSettings _settings;

    public CoinMarketCapService(
        HttpClient httpClient,
        IOptions<ExternalApiSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value.CoinMarketCap;

        _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        _httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", _settings.ApiKey);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<decimal> GetCryptoPriceInEurAsync(string symbol)
    {
        var response = await _httpClient.GetAsync($"cryptocurrency/quotes/latest?symbol={symbol}&convert=EUR");
        if (!response.IsSuccessStatusCode)
        {
            throw new ExternalApiException("CoinMarketCap API call failed.");
        }

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CoinMarketCapResponse>(content);

        if (result?.Data is null || !result.Data.ContainsKey(symbol))
        {
            throw new NotFoundException($"No data found for {symbol}");
        }

        var price = result.Data[symbol].Quote?["EUR"]?.Price;

        if (price is null)
        {
            throw new NotFoundException($"Price not available for {symbol} in EUR.");
        }

        return price.Value;
    }
}
