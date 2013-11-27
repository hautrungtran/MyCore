using System.Collections.Generic;
using System.Linq;
using MyCore.Collections;
using MyCore.Data;

namespace MyCore.Services {
    public abstract class RestorableObject<TKey> : TrackableObject<TKey> {
        public virtual bool Hidden { get; set; }
        public virtual bool Deleted { get; set; }
    }
    public interface IRestorableService<T, TKey> : ITrackableService<T, TKey> where T : RestorableObject<TKey> {
        IList<T> GetItems(bool showHidden);
        PagedList<T> GetItems(int pageIndex, int pageSize, bool showHidden);
        IList<T> GetDeletedItems();
        PagedList<T> GetDeletedItems(int pageIndex, int pageSize);
        T GetById(TKey id, bool showHidden);
        T GetByCode(string code, bool showHidden);
        bool Delete(TKey id, bool showHidden);
        TKey Show(T item);
        TKey Hide(T item);
    }
    public abstract class RestorableService<T, TKey> : TrackableService<T, TKey>, IRestorableService<T, TKey> where T : RestorableObject<TKey> {
        private readonly IRepositoryBase<T, TKey> _repository;
        protected RestorableService(IRepositoryBase<T, TKey> baseRepository)
            : base(baseRepository) {
            _repository = baseRepository;
        }
        
        #region Implementation of IRestorableService<T>
        
        public virtual IList<T> GetItems(bool showHidden) {
            return _repository.GetItems(entity => (showHidden || !entity.Hidden) && !entity.Deleted, entity => entity.UpdatedOn).ToList();
        }
        public virtual PagedList<T> GetItems(int pageIndex, int pageSize, bool showHidden) {
            var items = _repository.GetItems(entity => !entity.Deleted && (showHidden || !entity.Hidden)).OrderByDescending(entity => entity.CreatedOn).ThenByDescending(entity => entity.Id);
            return new PagedList<T>(items.Skip(pageSize * pageIndex).Take(pageSize).ToList(), items.Count);
        }
        public virtual IList<T> GetDeletedItems() {
            return _repository.GetItems(entity => entity.Deleted).OrderByDescending(entity => entity.CreatedOn).ThenByDescending(entity => entity.Id).ToList();
        }
        public virtual PagedList<T> GetDeletedItems(int pageIndex, int pageSize) {
            var items = _repository.GetItems(entity => entity.Deleted).OrderByDescending(entity => entity.CreatedOn).ThenByDescending(entity => entity.Id);
            return new PagedList<T>(items.Skip(pageSize * pageIndex).Take(pageSize).ToList(), items.Count);
        }
        public virtual T GetById(TKey id, bool showHidden) {
            return _repository.GetSingle(entity => (showHidden || !entity.Hidden) && !entity.Deleted && entity.Id.Equals(id));
        }
        public virtual T GetByCode(string code, bool showHidden) {
            return string.IsNullOrEmpty(code) ? null : _repository.GetSingle(entity => (showHidden || !entity.Hidden) && !entity.Deleted && entity.Code.ToUpper() == code.Trim().ToUpper());
        }
        public virtual bool Delete(TKey id, bool showHidden) {
            return Delete(GetById(id, showHidden));
        }
        public virtual TKey Show(T item) {
            item.Hidden = false;
            return base.Save(item);
        }
        public virtual TKey Hide(T item) {
            item.Hidden = true;
            return base.Save(item);
        }

        #endregion

        #region Overrides of TrackableService<T>

        public override IList<T> GetItems() {
            return GetItems(false);
        }
        public override PagedList<T> GetItems(int pageIndex, int pageSize) {
            return GetItems(pageIndex, pageSize, false);
        }
        public override T GetById(TKey id) {
            return GetById(id, false);
        }
        public override T GetByCode(string code) {
            return GetByCode(code, false);
        }
        public override bool Delete(TKey id) {
            return Delete(id, false);
        }
        public override bool Delete(T item) {
            if (item != null) {
                if (item.Deleted) {
                    return base.Delete(item);
                }
                item.Deleted = true;
                return !base.Save(item).Equals(default(TKey));
            }
            return false;
        }
        #endregion
    }
}