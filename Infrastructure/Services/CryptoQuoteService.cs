using CryptoQuoteApi.Application.Dtos;
using CryptoQuoteApi.Application.Interfaces;

namespace CryptoQuoteApi.Infrastructure.Services;

public class CryptoQuoteService : ICryptoQuoteService
{
    private readonly CoinMarketCapClient _coinClient;
    private readonly ExchangeRatesClient _exchangeClient;
    private readonly ILogger<CryptoQuoteService> _logger;

    public CryptoQuoteService(CoinMarketCapClient coinClient, ExchangeRatesClient exchangeClient, 
        ILogger<CryptoQuoteService> logger)
    {
        _coinClient = coinClient;
        _exchangeClient = exchangeClient;
        _logger = logger;
    }

    public async Task<CryptoQuoteResponse?> GetCryptoQuoteAsync(string symbol)
    {
        try
        {
            var priceInEur = await _coinClient.GetPriceInEurAsync(symbol);
            if (priceInEur is null)
            {
                _logger.LogWarning($"Price was NOT found for symbol {symbol}");
                return null;
            }

            var rates = await _exchangeClient.GetRatesFromEurAsync();
            var convertedRates = rates.ToDictionary(
                r => r.Key,
                r => priceInEur.Value * r.Value);

            var response = new CryptoQuoteResponse
            {
                Symbol = symbol.ToUpper(),
                Quotes = new Dictionary<string, decimal>
                {
                    { "EUR", priceInEur.Value },
                }
            };

            foreach (var cr in convertedRates)
            {
                response.Quotes.TryAdd(cr.Key, cr.Value);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting crypto quote for symbol {symbol}");
            return null;
        }
        
    }
}
