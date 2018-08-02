using System;
using System.Collections.Concurrent;

namespace BrainCorp.Passwd.Common.Caching
{
    public class InfiniteReferenceCaching : ICaching
    {

        public CacheItem Get(string cacheKey)
        {
            if( null == cacheKey) { return null;  }

            CacheItem obj = null;

            _cache.TryGetValue(cacheKey, out obj);

            return obj;
        }

        public void Insert(string cacheKey, object payload)
        {
            if (null == cacheKey) { throw new ArgumentNullException("The cacheKey cannot be NULL.", "cacheKey"); }
            if (null == payload) { throw new ArgumentNullException("The payload cannot be NULL.", "payload"); }

            CacheItem cacheItem = new CacheItem();
            cacheItem.Obj = payload;
            cacheItem.TimeAddedUTC = DateTime.UtcNow;

            _cache.AddOrUpdate(
                cacheKey,
                cacheItem,
                (oldKey, oldValue) => cacheItem);
        }

        private static ConcurrentDictionary<string, CacheItem> _cache =
           new ConcurrentDictionary<string, CacheItem>();
    }
}
