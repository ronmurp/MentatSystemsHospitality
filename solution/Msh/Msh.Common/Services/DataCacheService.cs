using Microsoft.Extensions.Caching.Memory;
using Msh.Common.Data;

namespace Msh.Common.Services;

/// <summary>
/// A MemoryCache service with ConfigDbContext source data
/// </summary>
/// <param name="memoryCache"></param>
public abstract class DataCacheService(IMemoryCache memoryCache, IConfigRepository configRepository) : IDataCacheService
{

    public async Task<T> GetData<T>(string cacheName, bool loadRaw = false)
    {
        if (loadRaw)
        {
            await Task.Delay(0);
            return configRepository.GetConfigContent<T>(cacheName);
        }

        if (!memoryCache.TryGetValue(cacheName, out T? cacheValue) || cacheValue == null)
        {
            cacheValue = configRepository.GetConfigContent<T>(cacheName); 

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(12)); // The expiration could be loaded from config from db object

            memoryCache.Set(cacheName, cacheValue, cacheEntryOptions);
        }

        return cacheValue;
    }

    public void Reload(string cacheName)
    {
        memoryCache.Remove(cacheName);
    }
}