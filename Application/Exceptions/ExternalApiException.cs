namespace CryptoQuoteApi.Application.Exceptions;

public class ExternalApiException : ApiException
{
    public ExternalApiException(string message)
        : base(message, 502, "External API Error")
    {
    }
}
