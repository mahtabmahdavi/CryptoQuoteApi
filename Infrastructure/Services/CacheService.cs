using CryptoQuoteApi.Application.Interfaces;
using CryptoQuoteApi.Application.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace CryptoQuoteApi.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly CacheSettings _settings;

    public CacheService(IMemoryCache cache, IOptions<CacheSettings> options)
    {
        _cache = cache;
        _settings = options.Value;
    }

    public T? Get<T>(string key)
    {
        return _cache.TryGetValue(key, out T? value) ? value : default;
    }

    public void Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(_settings.SlidingExpirationMinutes))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(_settings.AbsoluteExpirationMinutes));

        if (expiration.HasValue)
        {
            cacheEntryOptions.SetAbsoluteExpiration(expiration.Value);
        }

        _cache.Set(key, value, cacheEntryOptions);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}
