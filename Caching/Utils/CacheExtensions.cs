using Microsoft.Extensions.Caching.Distributed;

namespace Utils
{
    public static class CachingExtensions
    {

        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T obj, int? expirationSeconds = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationSeconds.HasValue
                    ? TimeSpan.FromSeconds(expirationSeconds.Value)
                    : null
            };
            await cache.SetStringAsync(key, System.Text.Json.JsonSerializer.Serialize(obj), options);
        }

        public static async Task<T?> GetAsync<T>(this IDistributedCache distributedCache, string key)
        {
            var value = await distributedCache.GetStringAsync(key);

            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            else
            {
                return System.Text.Json.JsonSerializer.Deserialize<T>(value);
            }
        }

        public static async Task<T?> GetAndSetCacheAsync<T>(this IDistributedCache distributedCache, string key, Func<Task<T>> populatingFunction)
        {
            var results = await distributedCache.GetAsync<T>(key);

            if (results != null) return results;

            results = await populatingFunction();

            if (results != null)
            {
                await distributedCache.SetAsync<T>(key, results);
            }

            return results;
        }

    }
}