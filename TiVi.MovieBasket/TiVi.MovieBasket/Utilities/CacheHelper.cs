using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace TiVi.MovieBasketService.Utilities
{
    public static class CacheHelper
    {
        public static void SetRecord<T>(this IDistributedCache cache, string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(900);
            options.SlidingExpiration = slidingExpireTime ?? TimeSpan.FromSeconds(900);

            var jsonData = JsonSerializer.Serialize(data);
            cache.SetString(recordId, jsonData, options);
        }

        public static T? GetRecord<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = cache.GetString(recordId);

            if (jsonData is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public static void RemoveRecord(this IDistributedCache cache, string recordId)
        {
            var jsonData = cache.GetString(recordId);

            if (jsonData is not null)
            {
                cache.Remove(recordId);
            }
        }
    }
}
