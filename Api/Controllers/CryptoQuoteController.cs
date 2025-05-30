using CryptoQuoteApi.Application.Dtos;
using CryptoQuoteApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoQuoteApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CryptoQuoteController : ControllerBase
{
    private readonly ICryptoQuoteService _cryptoQuoteService;
    private readonly ILogger<CryptoQuoteController> _logger;

    public CryptoQuoteController(
        ICryptoQuoteService cryptoQuoteService,
        ILogger<CryptoQuoteController> logger)
    {
        _cryptoQuoteService = cryptoQuoteService;
        _logger = logger;
    }

    [HttpGet("{symbol}")]
    public async Task<ActionResult<CryptoQuoteResponse> GetQuote(string symbol)
    {
        try
        {
            _logger.LogInformation($"Getting quote for symbol: {symbol}");
            var result = await _cryptoQuoteService.GetCryptoQuoteAsync(symbol);

            if (result is null)
            {
                return NotFound(new
                {
                    Message = $"No data found for {symbol.ToUpper()}"
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting quote for symbol: {symbol}");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
