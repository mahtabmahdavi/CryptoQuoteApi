namespace CryptoQuoteApi.Application.Interfaces;

public interface ICoinMarketCapService
{
    Task<decimal> GetCryptoPriceInEurAsync(string symbol);
}
