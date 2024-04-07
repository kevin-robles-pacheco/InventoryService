using InventoryService.Infraestructure.Services.Cache.Contract;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;

namespace InventoryService.Infraestructure.Services.Cache.Implementation
{
    public class MemoryCacheManager : ICache
    {
        private readonly MemoryCache _cache;

        public MemoryCacheManager()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public bool TryGetFromCache<T>(string cacheKey, out T? value)
        {
            var existInCache = _cache.TryGetValue(cacheKey, out T? cacheValue);
            value = cacheValue;
            return existInCache;
        }

        public void SaveToCacheWithExpiration<T>(string cacheKey, T value, TimeSpan expiration)
        {
            try
            {
                var expirationConfig = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration
                };

                _cache.Set(cacheKey, value, expirationConfig);

                if (TryGetFromCache<string>(cacheKey, out string savedValue))
                    Console.WriteLine($"Cache for DataBase saved successfull for: [{expiration.TotalMinutes}] minutes.");
                else
                    Console.WriteLine($"Error, no cache saved for key: [{cacheKey}]");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving Cache: {ex.Message}");
                throw;
            }
            
        }
    }
}
