using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCollectionExtension.Configuration.Cache
{
    public class InMemoryCache : ICache
    {
        private readonly IMemoryCache _cache;

        public InMemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }
        public T Get<T>(string key)
        {
            if (_cache.TryGetValue<T>(key, out var obj))
            {
                return obj;
            }
            return default(T);
        }

        public void Set<T>(string key, T value)
        {
            _cache.Set(key, value);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
