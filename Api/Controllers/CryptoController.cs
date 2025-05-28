using CryptoQuoteApi.Application.Dtos;
using CryptoQuoteApi.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoQuoteApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CryptoController : ControllerBase
{
    private readonly CoinMarketCapClient _client;

    public CryptoController(CoinMarketCapClient client)
    {
        _client = client;
    }

    [HttpGet("{symbol}")]
    public async Task<IActionResult> GetPrice(string symbol)
    {
        var price = await _client.GetPriceInUsdAsync(symbol);

        if (price == null)
        {
            return NotFound(new
            {
                Message = $"No data found for {symbol.ToUpper()}"
            });
        }

        var response = new CryptoQuoteResponseDto
        {
            Symbol = symbol.ToUpper(),
            Rates = new Dictionary<string, decimal>
            {
                { "USD", price.Value }
            }
        };

        return Ok(response);
    }
}
