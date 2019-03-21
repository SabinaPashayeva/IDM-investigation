using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Client_App.Services
{
    public class AppMemoryCache : IAppMemoryCache
    {
        private readonly IMemoryCache _cache;

        public AppMemoryCache(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public T TryGetValue<T>()
        {
            if (!_cache.TryGetValue(CacheKeys.Entry, out T cachedItem))
            {
                return default(T);
            }

            return cachedItem;
        }

        public void SetValue<T>(T needsCachingItem)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(15));

            _cache.Set<T>(CacheKeys.Entry, needsCachingItem, cacheEntryOptions);
        }
    }
}
