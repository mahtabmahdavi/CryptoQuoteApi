using CryptoQuoteApi.Application.Dtos;

namespace CryptoQuoteApi.Application.Interfaces;

public interface ICryptoQuoteService
{
    Task<CryptoQuoteResponseDto?> GetCryptoQuoteAsync(string symbol);
}
