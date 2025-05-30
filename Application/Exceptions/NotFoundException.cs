namespace CryptoQuoteApi.Application.Exceptions;

public class NotFoundException : ApiException
{
    public NotFoundException(string message)
        : base(message, 404, "NOT_FOUND")
    {
    }
}
