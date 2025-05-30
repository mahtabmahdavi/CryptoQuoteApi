using CryptoQuoteApi.Application.Dtos;

namespace CryptoQuoteApi.Application.Interfaces;

public interface ICryptoQuoteService
{
    Task<CryptoQuoteResponse?> GetCryptoQuoteAsync(string symbol);
}
