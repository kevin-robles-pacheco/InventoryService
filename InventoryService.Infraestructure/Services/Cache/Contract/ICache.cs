namespace InventoryService.Infraestructure.Services.Cache.Contract
{
    public interface ICache
    {
        public bool TryGetFromCache<T>(string cacheKey, out T? value);
        public void SaveToCacheWithExpiration<T>(string cacheKey, T value, TimeSpan expiration);
    }
}
