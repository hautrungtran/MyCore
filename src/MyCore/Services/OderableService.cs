using System.Collections.Generic;
using System.Linq;
using MyCore.Collections;
using MyCore.Data;

namespace MyCore.Services {
    public class OrderableObject<TKey> : RestorableObject<TKey> {
        public int Order { get; set; }
    }
    public interface IOrderableService<T, TKey> : IRestorableService<T, TKey> where T : OrderableObject<TKey> {
    }
    public abstract class OrderableService<T, TKey> : RestorableService<T, TKey>, IOrderableService<T, TKey> where T : OrderableObject<TKey> {
        private readonly IRepositoryBase<T, TKey> _repository;
        protected OrderableService(IRepositoryBase<T, TKey> baseRepository)
            : base(baseRepository) {
            _repository = baseRepository;
        }

        #region Overrides of ResoterableService<T>

        public override IList<T> GetItems(bool showHidden) {
            return _repository.GetItems(entity => !entity.Deleted && (showHidden || !entity.Hidden)).OrderByDescending(entity => entity.Order).ThenByDescending(entity => entity.CreatedOn).ToList();
        }
        public override PagedList<T> GetItems(int pageIndex, int pageSize, bool showHidden) {
            var items = _repository.GetItems(entity => !entity.Deleted && (showHidden || !entity.Hidden)).OrderByDescending(entity => entity.Order).ThenByDescending(entity => entity.CreatedOn);
            return new PagedList<T>(items.Skip(pageSize * pageIndex).Take(pageSize).ToList(), items.Count);
        }
        public override IList<T> GetDeletedItems() {
            return _repository.GetItems(entity => entity.Deleted).OrderByDescending(entity => entity.Order).ThenByDescending(entity => entity.CreatedOn).ToList();
        }
        public override PagedList<T> GetDeletedItems(int pageIndex, int pageSize) {
            var items = _repository.GetItems(entity => entity.Deleted).OrderByDescending(entity => entity.Order).ThenByDescending(entity => entity.CreatedOn);
            return new PagedList<T>(items.Skip(pageSize * pageIndex).Take(pageSize).ToList(), items.Count);
        }

        #endregion
    }
}