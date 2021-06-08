using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Newtonsoft.Json;
using System;

namespace Application.Services
{
    public class AppConfigService : IAppConfigService
    {
        private readonly IRepository<AppConfig> _appConfigRepository;

        public AppConfigService(IRepository<AppConfig> appConfigRepository)
        {
            _appConfigRepository = appConfigRepository;
        }

        public bool Exists(string key)
        {
            return this.GetValue(key) != null;
        }

        public T Get<T>(string key)
        {
            var config = this.GetValue(key);

            if (config == null || string.IsNullOrEmpty(config.Value))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(config.Value);
        }

        public string Get(string key)
        {
            var config = this.GetValue(key);

            if (config == null || string.IsNullOrEmpty(config.Value))
            {
                return null;
            }
            return config.Value;
        }

        private AppConfig GetValue(string key)
        {
            var config = this._appConfigRepository.Get(item => item.Key == key);
            return config;
        }

        public void Put<T>(string key, T value)
        {
            var config = this.GetValue(key);

            var configValue = JsonConvert.SerializeObject(value);
            if (config == null)
            {
                this._appConfigRepository.Create(new AppConfig() { Key = key, Value = configValue });
            }
            else
            {
                config.Value = configValue;
            }
            this._appConfigRepository.Flush();
        }

        public T GetOrAdd<T>(string key, Func<T> factory)
        {
            if (Exists(key))
            {
                return Get<T>(key);
            }

            var value = factory();
            Put(key, value);
            return value;
        }

        public T Get<T>(string key, string isoCountryCode)
        {
            var countryKey = this.GetCountryBasedKey(key, isoCountryCode);
            var countryConfig = this.GetValue(countryKey);
            if (countryConfig != null)
            {
                return JsonConvert.DeserializeObject<T>(countryConfig.Value);
            }
            else
            {
                return this.Get<T>(key);
            }
        }

        private string GetCountryBasedKey(string key, string isoCountryCode)
        {
            return $"Country_{isoCountryCode}_{key}";
        }
    }
}