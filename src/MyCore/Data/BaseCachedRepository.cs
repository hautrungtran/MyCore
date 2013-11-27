using System.Collections.Generic;
using System.Linq;
using MyCore.Caching;

namespace MyCore.Data {
    public abstract class BaseCachedRepository<T, TKey> : RepositoryBase<T, TKey> where T : DomainObject<TKey> {
        private readonly ICache _cache;
        protected virtual string CacheKey { get { return GetType().FullName; } }
        protected virtual int CacheTime { get { return 3600; } }
        protected override IQueryable<T> Entities {
            get {
                if (!_cache.ContainsKey(CacheKey)) {
                    _cache.AddOrUpdate(CacheKey, Context.GetAll<T, TKey>().ToList(), CacheTime);
                }
                return _cache.Get<List<T>>(CacheKey).AsQueryable();
            }
        }
        protected BaseCachedRepository(IDatabaseContext context, ICache cache)
            : base(context) {
            _cache = cache;
        }
        public override T GetSingle(TKey id) {
            return GetSingle(entity => entity.Id.Equals(id));
        }
        public override TKey Insert(T entity) {
            var result = base.Insert(entity);
            _cache.Remove(item => item.Key == CacheKey);
            return result;
        }
        public override TKey Update(T entity) {
            var result = base.Update(entity);
            _cache.Remove(item => item.Key == CacheKey);
            return result;
        }
        public override bool Delete(T entity) {
            var result = base.Delete(entity);
            if (result) {
                _cache.Remove(item => item.Key == CacheKey);
            }
            return result;
        }
    }
}