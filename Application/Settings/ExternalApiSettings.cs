namespace CryptoQuoteApi.Application.Settings;

public class ExternalApiSettings
{
    public CoinMarketCapSettings CoinMarketCap { get; set; }
    public ExchangeRatesSettings ExchangeRates { get; set; }
}
