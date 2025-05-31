namespace CryptoQuoteApi.Application.Settings;

public class CacheSettings
{
    public int DefaultExpirationMinutes { get; set; }
    public int SlidingExpirationMinutes { get; set; }
    public int AbsoluteExpirationMinutes { get; set; }
}
