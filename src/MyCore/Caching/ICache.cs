using System;
using System.Collections.Generic;

namespace MyCore.Caching {
    public interface ICache {
        object this[string key] { get; }
        bool ContainsKey(string key);
        object GetValue(string key);
        IDictionary<string, object> GetValue(Func<KeyValuePair<string, object>, bool> func);
        T Get<T>(string key);
        IList<T> GetList<T>(string key);
        void AddOrUpdate(string key, object value, int cacheTime = 0);
        void Remove(string key);
        void Remove(Func<KeyValuePair<string, object>, bool> func);
        void Clear();
    }
}