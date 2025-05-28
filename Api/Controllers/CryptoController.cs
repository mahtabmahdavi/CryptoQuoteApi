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
        return Ok();
    }
}
