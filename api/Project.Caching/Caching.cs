using Project.Caching.Interfaces;
using System.Runtime.Caching;

namespace Project.Caching
{
    public class Caching : ICaching
    {
        private ObjectCache Cache { get { return MemoryCache.Default; } }


        /// <summary>
        /// Set Cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"> cache time by seconds</param>
        public void Set(string key, object data, int cacheTime)
        {
            CacheItemPolicy policy = new CacheItemPolicy();

            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(cacheTime);

            if (data != null)
            {
                Cache.Add(new CacheItem(key, data), policy);
            }

        }

        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        public void Delete(string key)
        {
            try
            {
                Cache.Remove(key);
            }
            catch (Exception)
            {
            }

        }

        public bool Exists(string key)
        {
            try
            {
                return Cache.Get(key) != null;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
