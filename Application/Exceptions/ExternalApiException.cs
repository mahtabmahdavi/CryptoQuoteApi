namespace CryptoQuoteApi.Application.Exceptions;

public class ExternalApiException : ApiException
{
    public ExternalApiException(string message)
        : base(message, 502, "EXTERNAL_API_ERROR")
    {
    }
}
