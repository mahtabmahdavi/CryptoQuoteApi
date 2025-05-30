using CryptoQuoteApi.Application.Dtos;
using CryptoQuoteApi.Application.Interfaces;

namespace CryptoQuoteApi.Infrastructure.Services;

public class CryptoQuoteService : ICryptoQuoteService
{
    private readonly CoinMarketCapService _coinService;
    private readonly ExchangeRatesService _exchangeClient;
    private readonly ILogger<CryptoQuoteService> _logger;

    public CryptoQuoteService(
        CoinMarketCapService coinService,
        ExchangeRatesService exchangeClient, 
        ILogger<CryptoQuoteService> logger)
    {
        _coinService = coinService;
        _exchangeClient = exchangeClient;
        _logger = logger;
    }

    public async Task<CryptoQuoteResponse> GetCryptoQuoteAsync(string symbol)
    {
        try
        {
            var eurPrice = await _coinService.GetCryptoPriceInEurAsync(symbol);
            var rates = await _exchangeClient.GetExchangeRatesFromEurAsync();
            var convertedRates = rates.ToDictionary(
                r => r.Key,
                r => eurPrice * r.Value);

            var response = new CryptoQuoteResponse
            {
                Symbol = symbol.ToUpper(),
                Quotes = new Dictionary<string, decimal>()
            };

            foreach (var cr in convertedRates)
            {
                response.Quotes.TryAdd(cr.Key, cr.Value);
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting crypto quote for symbol: {symbol}");
            throw;
        }
    }
}
