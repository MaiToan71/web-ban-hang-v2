using Project.Caching.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Caching
{
    public class CachingExtension : ICachingExtension
    {
        private readonly ICaching _cache;
        public CachingExtension(ICaching caching)
        {
            _cache = caching;
        }

        private static List<string> _KeyCaches = new List<string>();

        private static object _lock = new object();
        public List<string> KeyCaches { get { return _KeyCaches; } }

        public void SetCache(string key, object obj, int seconds)
        {
            try
            {
                if (obj != null)
                {
                    _cache.Set(key, obj, seconds);
                    lock (_lock)
                    {
                        if (!_KeyCaches.Contains(key))
                        {
                            _KeyCaches.Add(key);
                        }
                    }


                }
            }
            catch (Exception)
            {
            }
        }

        public bool TryGetCache<T>(out T obj, string key) where T : new()
        {
            try
            {
                if (_cache.Exists(key))
                {
                    obj = _cache.Get<T>(key);
                    return true;
                }
            }
            catch (Exception)
            {
            }

            obj = new T();

            return false;
        }

        public void DeleteAll()
        {
            lock (_lock)
            {
                _KeyCaches.ForEach(x => _cache.Delete(x));
            }

        }

        public void DeleteCache(string key)
        {
            try
            {
                if (_cache.Exists(key))
                {
                    _cache.Delete(key);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
