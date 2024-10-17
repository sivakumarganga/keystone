using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KeyStone.Shared
{
    public static class DistributedCacheExtensions
    {
        public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key)
        {
            var data = await cache.GetAsync(key);
            if (data == null)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(data);
        }
        public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
        {
            var data = JsonSerializer.Serialize(value);
            await cache.SetAsync(key, Encoding.UTF8.GetBytes(data), options);
        }
        public static async Task SetItemAsync(this IDistributedCache cache, string key, object value)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions() { AbsoluteExpiration=DateTimeOffset.UtcNow.AddMinutes(30) };
            var data = JsonSerializer.Serialize(value);
            await cache.SetAsync(key, Encoding.UTF8.GetBytes(data), options);
        }
    }
}
