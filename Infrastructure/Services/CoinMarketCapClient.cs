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

}
