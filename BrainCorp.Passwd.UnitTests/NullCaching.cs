using BrainCorp.Passwd.Common.Caching;

namespace BrainCorp.Passwd.Testing.UnitTests
{
    public class NullCaching : ICaching
    {
        public CacheItem Get(string cacheKey)
        {
            return null;
        }

        public void Insert(string cacheKey, object payload)
        {
           
        }
    }
}
