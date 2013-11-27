using System.Collections.Generic;
using System.Linq;
using MyCore.Collections;
using MyCore.Data;

namespace MyCore.Services {
    public class IdentityObject<TKey> : DomainObject<TKey> { }
    public interface IServiceBase<T, TKey> where T : IdentityObject<TKey> {
        IList<T> GetItems();
        PagedList<T> GetItems(int pageIndex, int pageSize);
        T GetById(TKey id);
        TKey Save(T item);
        bool Delete(T item);
        bool Delete(TKey id);
    }
    public abstract class ServiceBase<T, TKey> : IServiceBase<T, TKey> where T : IdentityObject<TKey> {
        private readonly IRepositoryBase<T, TKey> _repository;
        protected ServiceBase(IRepositoryBase<T, TKey> baseRepository) {
            _repository = baseRepository;
        }
        #region Implementation of IBaseService<T>

        public virtual IList<T> GetItems() {
            return _repository.GetItems(entity => entity.Id).ToList();
        }
        public virtual PagedList<T> GetItems(int pageIndex, int pageSize) {
            var items = _repository.GetItems(entity => entity.Id);
            // ReSharper disable PossibleMultipleEnumeration
            return new PagedList<T>(items.Skip(pageIndex * pageSize).Take(pageSize).ToList(), items.Count);
            // ReSharper restore PossibleMultipleEnumeration
        }
        public virtual T GetById(TKey id) {
            return _repository.GetSingle(id);
        }
        public virtual TKey Save(T item) {
            return item.Id.Equals(default(T)) ? _repository.Insert(item) : _repository.Update(item);
        }
        public virtual bool Delete(T item) {
            return _repository.Delete(item);
        }
        public virtual bool Delete(TKey id) {
            return _repository.Delete(id);
        }

        #endregion

    }
}