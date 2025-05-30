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
    private readonly ILogger<CryptoQuoteController> _logger;

    public CryptoQuoteController(
        ICryptoQuoteService cryptoQuoteService,
        IValidator<CryptoQuoteRequest> validator,
        ILogger<CryptoQuoteController> logger)
    {
        _cryptoQuoteService = cryptoQuoteService;
        _validator = validator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<CryptoQuoteResponse>> GetQuote([FromBody] CryptoQuoteRequest request)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            _logger.LogInformation($"Getting quote for symbol: {request.Symbol}");
            var result = await _cryptoQuoteService.GetCryptoQuoteAsync(request.Symbol);

            if (result is null)
            {
                return NotFound(new
                {
                    Message = $"No data found for {request.Symbol}"
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting quote for symbol: {request.Symbol}");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
