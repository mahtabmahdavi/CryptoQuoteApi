using CryptoQuoteApi.Application.Dtos;
using FluentValidation;

namespace CryptoQuoteApi.Application.Validators;

public class CryptoQuoteRequestValidator : AbstractValidator<CryptoQuoteRequest>
{
    public CryptoQuoteRequestValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty()
            .WithMessage("Symbol is required")
            .MaximumLength(10)
            .WithMessage("Symbol must not exceed 10 characters")
            .Matches("^[a-zA-Z]+$")
            .WithMessage("Symbol must contain only alphabetic letters (A-Z or a-z).");
    }
}
