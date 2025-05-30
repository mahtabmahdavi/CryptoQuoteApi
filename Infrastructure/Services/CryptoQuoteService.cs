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

    public async Task<CryptoQuoteResponse> GetCryptoQuoteAsync(CryptoQuoteRequest request)
    {
        var eurPrice = await _coinService.GetCryptoPriceInEurAsync(request.Symbol);
        var rates = await _exchangeClient.GetExchangeRatesFromEurAsync();
        var convertedRates = rates.ToDictionary(
            r => r.Key,
            r => eurPrice * r.Value);

        return new CryptoQuoteResponse
        {
            Symbol = request.Symbol.ToUpper(),
            Quotes = convertedRates
        };
    }
}
