using CryptoQuoteApi.Application.Dtos;
using CryptoQuoteApi.Application.Interfaces;

namespace CryptoQuoteApi.Infrastructure.Services;

public class CryptoQuoteService : ICryptoQuoteService
{
    private readonly ICoinMarketCapService _coinService;
    private readonly IExchangeRatesService _exchangeService;
    private readonly ILogger<CryptoQuoteService> _logger;
    private readonly ICacheService _cacheService;
    private const string EXCHANGE_RATES_CACHE_KEY = "exchange_rates";
    private const string CRYPTO_PRICE_CACHE_KEY = "crypto_price_{0}";

    public CryptoQuoteService(
        ICoinMarketCapService coinService,
        IExchangeRatesService exchangeService,
        ILogger<CryptoQuoteService> logger,
        ICacheService cacheService)
    {
        _coinService = coinService;
        _exchangeService = exchangeService;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<CryptoQuoteResponse> GetCryptoQuoteAsync(CryptoQuoteRequest request)
    {
        var rates = await GetExchangeRatesAsync();
        var eurPrice = await GetCryptoPriceAsync(request.Symbol);

        var convertedRates = rates.ToDictionary(
            r => r.Key,
            r => eurPrice * r.Value);

        return new CryptoQuoteResponse
        {
            Symbol = request.Symbol.ToUpper(),
            Quotes = convertedRates,
            LastUpdated = DateTime.UtcNow
        };
    }

    private async Task<Dictionary<string, decimal>> GetExchangeRatesAsync()
    {
        var cachedRates = _cacheService.Get<Dictionary<string, decimal>>(EXCHANGE_RATES_CACHE_KEY);
        if (cachedRates != null)
        {
            _logger.LogInformation("Retrieved exchange rates from cache");
            return cachedRates;
        }

        var rates = await _exchangeService.GetExchangeRatesFromEurAsync();

        _cacheService.Set(EXCHANGE_RATES_CACHE_KEY, rates);
        _logger.LogInformation("Stored exchange rates in cache");

        return rates;
    }

    private async Task<decimal> GetCryptoPriceAsync(string symbol)
    {
        var cacheKey = string.Format(CRYPTO_PRICE_CACHE_KEY, symbol.ToUpper());

        var cachedPrice = _cacheService.Get<decimal>(cacheKey);
        if (cachedPrice != default)
        {
            _logger.LogInformation($"Retrieved {symbol.ToUpper()} price from cache");
            return cachedPrice;
        }

        var price = await _coinService.GetCryptoPriceInEurAsync(symbol);

        _cacheService.Set(cacheKey, price);
        _logger.LogInformation($"Stored {symbol.ToUpper()} price in cache");

        return price;
    }
}
