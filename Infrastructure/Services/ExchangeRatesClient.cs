namespace CryptoQuoteApi.Infrastructure.Services;

public class ExchangeRatesClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExchangeRatesClient> _logger;

    public ExchangeRatesClient(IHttpClientFactory httpClientFactory, ILogger<ExchangeRatesClient> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ExchangeRates");
        _logger = logger;
    }
}
