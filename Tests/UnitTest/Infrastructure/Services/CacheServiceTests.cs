using CryptoQuoteApi.Application.Settings;
using CryptoQuoteApi.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.UnitTest.Infrastructure.TestModels;

namespace Tests.UnitTest.Infrastructure.Services;

[TestClass]
public class CacheServiceTests
{
    private IMemoryCache _memoryCache;
    private CacheSettings _cacheSettings;
    private CacheService _cacheService;

    [TestInitialize]
    public void Setup()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _cacheSettings = new CacheSettings
        {
            DefaultExpirationMinutes = 5,
            SlidingExpirationMinutes = 2,
            AbsoluteExpirationMinutes = 30
        };

        var options = Options.Create(_cacheSettings);
        _cacheService = new CacheService(_memoryCache, options);
    }

    [TestMethod]
    public void Get_WhenKeyExists_ReturnsValue()
    {
        // Arrange
        var key = "test_key";
        var expectedValue = "test_value";
        _cacheService.Set(key, expectedValue);

        // Act
        var result = _cacheService.Get<string>(key);

        // Assert
        Assert.AreEqual(expectedValue, result);
    }


    [TestMethod]
    public void Get_WhenKeyDoesNotExist_ReturnsDefault()
    {
        // Arrange
        var key = "non_existent_key";

        // Act
        var result = _cacheService.Get<string>(key);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void Set_WithDefaultExpiration_StoresValue()
    {
        // Arrange
        var key = "test_key";
        var value = "test_value";

        // Act
        _cacheService.Set(key, value);

        // Assert
        var result = _cacheService.Get<string>(key);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void Set_WithCustomExpiration_StoresValue()
    {
        // Arrange
        var key = "test_key";
        var value = "test_value";
        var expiration = TimeSpan.FromMinutes(10);

        // Act
        _cacheService.Set(key, value, expiration);

        // Assert
        var result = _cacheService.Get<string>(key);
        Assert.AreEqual(value, result);
    }

    [TestMethod]
    public void Set_WithNullValue_StoresNull()
    {
        // Arrange
        var key = "test_key";
        string value = null;

        // Act
        _cacheService.Set(key, value);
        var result = _cacheService.Get<string>(key);

        // Assert
        Assert.IsNull(result);
    }
}
