namespace CryptoQuoteApi.Application.Exceptions;

public class BadRequestException : ApiException
{
    public BadRequestException(string message)
        : base(message, 400, "Bad Request")
    {
    }
}
