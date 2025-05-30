using CryptoQuoteApi.Application.Dtos;
using CryptoQuoteApi.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CryptoQuoteApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CryptoQuoteController : ControllerBase
{
    private readonly ICryptoQuoteService _cryptoQuoteService;
    private readonly IValidator<CryptoQuoteRequest> _validator;

    public CryptoQuoteController(
        ICryptoQuoteService cryptoQuoteService,
        IValidator<CryptoQuoteRequest> validator)
    {
        _cryptoQuoteService = cryptoQuoteService;
        _validator = validator;
    }

    [HttpPost]
    public async Task<ActionResult<CryptoQuoteResponse>> GetQuote([FromBody] CryptoQuoteRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var result = await _cryptoQuoteService.GetCryptoQuoteAsync(request);
        return Ok(result); 
    }
}
