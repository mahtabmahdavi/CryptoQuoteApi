namespace CryptoQuoteApi.Application.Dtos;

public class CryptoQuoteResponseDto
{
    public string Symbol { get; set; } = string.Empty;
    public Dictionary<string, decimal> Rates { get; set; } = new();
}
