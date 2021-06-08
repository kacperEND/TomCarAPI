using System;

namespace Application.Interfaces
{
    public interface IAppConfigService
    {
        bool Exists(string key);

        T Get<T>(string key);

        string Get(string key);

        T GetOrAdd<T>(string key, Func<T> factory);

        void Put<T>(string key, T value);
    }
}