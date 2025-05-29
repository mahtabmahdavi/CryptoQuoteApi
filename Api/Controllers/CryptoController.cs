using CryptoQuoteApi.Application.Interfaces;
using CryptoQuoteApi.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoQuoteApi.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CryptoController : ControllerBase
{
    private readonly ICryptoQuoteService _service;

    public CryptoController(ICryptoQuoteService service)
    {
        _service = service;
    }

    [HttpGet("{symbol}")]
    public async Task<IActionResult> GetPrice(string symbol)
    {
        var response = await _service.GetCryptoQuoteAsync(symbol);

        if (response == null)
        {
            return NotFound(new
            {
                Message = $"No data found for {symbol.ToUpper()}"
            });
        }

        return Ok(response);
    }
}
