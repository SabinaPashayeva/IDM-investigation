using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Services
{
    public interface IAppMemoryCache
    {
        T TryGetValue<T>(string keyEntry);
        void SetValue<T>(string keyEntry, T needsCachingItem);
    }
}
