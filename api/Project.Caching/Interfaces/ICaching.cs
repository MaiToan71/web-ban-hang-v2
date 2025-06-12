using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Caching.Interfaces
{
    public interface ICaching
    {
        void Delete(string key);
        bool Exists(string key);
        T Get<T>(string key);
        void Set(string key, object data, int cacheTime);
    }
}
