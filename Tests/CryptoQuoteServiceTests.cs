using CryptoQuoteApi.Application.Dtos;
using CryptoQuoteApi.Application.Interfaces;
using CryptoQuoteApi.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests;

[TestClass]
public class CryptoQuoteServiceTests
{
    private Mock<ICoinMarketCapService> _mockCoinService;
    private Mock<IExchangeRatesService> _mockExchangeService;
    private Mock<ICacheService> _mockCacheService;
    private Mock<ILogger<CryptoQuoteService>> _mockLogger;
    private CryptoQuoteService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockCoinService = new Mock<ICoinMarketCapService>();
        _mockExchangeService = new Mock<IExchangeRatesService>();
        _mockCacheService = new Mock<ICacheService>();
        _mockLogger = new Mock<ILogger<CryptoQuoteService>>();

        _service = new CryptoQuoteService(
            _mockCoinService.Object,
            _mockExchangeService.Object,
            _mockLogger.Object,
            _mockCacheService.Object
        );
    }

    [TestMethod]
    public async Task GetCryptoQuoteAsync_ReturnsExpectedResult()
    {
        // Arrange
        var request = new CryptoQuoteRequest { Symbol = "BTC" };
        var exchangeRates = new Dictionary<string, decimal>
        {
            { "USD", 1.1m },
            { "EUR", 1.0m },
            { "GBP", 0.9m }
        };
        decimal btcPriceInEur = 30000m;

        _mockCacheService.Setup(c => c.Get<Dictionary<string, decimal>>(It.IsAny<string>()))
            .Returns((Dictionary<string, decimal>)null);

        _mockCacheService.Setup(c => c.Get<decimal>(It.IsAny<string>()))
            .Returns(0m);

        _mockExchangeService.Setup(x => x.GetExchangeRatesFromEurAsync())
            .ReturnsAsync(exchangeRates);

        _mockCoinService.Setup(x => x.GetCryptoPriceInEurAsync("BTC"))
            .ReturnsAsync(btcPriceInEur);

        // Act
        var result = await _service.GetCryptoQuoteAsync(request);

        // Assert
        Assert.AreEqual("BTC", result.Symbol);
        Assert.AreEqual(3, result.Quotes.Count);
        Assert.AreEqual(33000m, result.Quotes["USD"]);
        Assert.AreEqual(27000m, result.Quotes["GBP"]);
    }
}
