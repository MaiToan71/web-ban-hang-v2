using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Caching.Interfaces
{
    public interface ICachingExtension
    {
        bool TryGetCache<T>(out T obj, string key) where T : new();
        void SetCache(string key, object obj, int seconds);
        void DeleteCache(string key);
        void DeleteAll();
    }
}