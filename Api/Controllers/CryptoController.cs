using CryptoQuoteApi.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoQuoteApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CryptoController : ControllerBase
{
    private readonly ExchangeRatesClient _client;

    public CryptoController(ExchangeRatesClient client)
    {
        _client = client;
    }

    [HttpGet("{symbol}")]
    public async Task<IActionResult> GetPrice(string symbol)
    {
        var rates = await _client.GetRatesFromEurAsync();

        if (rates == null)
        {
            return NotFound(new
            {
                Message = $"No data found for {symbol.ToUpper()}"
            });
        }

        return Ok(rates);
    }
}
