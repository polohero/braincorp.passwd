
namespace BrainCorp.Passwd.Common.Caching
{
    public interface ICaching
    {
        CacheItem Get(string cacheKey);

        void Insert(string cacheKey, object payload);
    }
}
