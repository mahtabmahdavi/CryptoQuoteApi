namespace CryptoQuoteApi.Application.Interfaces;

public interface IExchangeRatesService
{
    Task<Dictionary<string, decimal>> GetExchangeRatesFromEurAsync();
}
