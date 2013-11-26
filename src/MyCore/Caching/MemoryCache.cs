using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace MyCore.Caching {
    public class MemoryCache : ICache {
        private readonly ObjectCache _cache = System.Runtime.Caching.MemoryCache.Default;

        #region Implementation of ICacheManager

        public bool ContainsKey(string key) { return _cache.Contains(key); }
        public object this[string key] { get { return GetValue(key); } }
        public object GetValue(string key) { return _cache[key]; }
        public IDictionary<string, object> GetValue(Func<KeyValuePair<string, object>, bool> func) {
            return _cache.Where(func).ToDictionary(item => item.Key, item => item.Value);
        }
        public T Get<T>(string key) { return ContainsKey(key) ? (T)GetValue(key) : default(T); }
        public IList<T> GetList<T>(string key) { return ContainsKey(key) ? ((IEnumerable)GetValue(key)).Cast<T>().ToList() : new List<T>(); }
        public void AddOrUpdate(string key, object value, int cacheTime = 0) {
            if (value == null) {
                Remove(key);
                return;
            }
            var dateTimeOffset = cacheTime > 0 ? DateTime.Now.AddSeconds(cacheTime) : ObjectCache.InfiniteAbsoluteExpiration;
            Remove(key);
            _cache.Add(key, value, dateTimeOffset);
        }
        public void Remove(string key) { _cache.Remove(key); }
        public void Remove(Func<KeyValuePair<string, object>, bool> func) {
            var keysToRemove = _cache.Where(func).Select(x => x.Key).ToList();
            foreach (var key in keysToRemove) {
                Remove(key);
            }
        }
        public void Clear() {
            foreach (var item in _cache) {
                Remove(item.Key);
            }
        }

        #endregion
    }
}