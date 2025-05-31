﻿namespace CryptoQuoteApi.Domain.Models;

public class CryptoQuote
{
    public string Symbol { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }
}
