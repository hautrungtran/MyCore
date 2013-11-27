using System.Collections.Generic;
using System.Linq;
using MyCore.Collections;
using MyCore.Data;

namespace MyCore.Services {
    public class HierachyObject<TKey> : OrderableObject<TKey> {
        public TKey ParentId { get; set; }
    }
    public interface IHierachyService<T, TKey> : IOrderableService<T, TKey> where T : HierachyObject<TKey> {
        IList<T> GetRoots(bool showHidden = false);
        IList<T> GetLeafts(bool showHidden = false);
        IList<T> GetParents(T item);
        IList<T> GetParents(TKey id, bool showHidden = false);
        IList<T> GetChildren(TKey id, bool showHidden = false);
        TreeNode<T> GetBranch(TKey id, bool showHidden = false);
    }
    public abstract class HierachyService<T, TKey> : OrderableService<T, TKey>, IHierachyService<T, TKey> where T : HierachyObject<TKey> {
        private readonly IRepositoryBase<T, TKey> _repository;
        protected HierachyService(IRepositoryBase<T, TKey> baseRepository)
            : base(baseRepository) {
            _repository = baseRepository;
        }
        #region Implementation of IHierachyService<T>

        public IList<T> GetRoots(bool showHidden = false) {
            return _repository.GetItems(entity => !entity.Deleted && (showHidden || !entity.Hidden) && entity.ParentId.Equals(default(TKey))).OrderByDescending(entity => entity.Order).ThenByDescending(entity => entity.CreatedOn).ToList();
        }
        public IList<T> GetLeafts(bool showHidden = false) {
            var items = _repository.GetItems(entity => !entity.Deleted && (showHidden || !entity.Hidden)).ToList();
            return items.Where(item => items.All(entity => !entity.ParentId.Equals(item.Id))).OrderByDescending(entity => entity.ParentId).ThenByDescending(entity => entity.Order).ThenByDescending(entity => entity.CreatedOn).ToList();
        }
        public IList<T> GetParents(T item) {
            var result = new List<T>();
            if (item != null) {
                var parentId = item.ParentId;
                while (!parentId.Equals(default(TKey))) {
                    var parent = GetById(parentId, true);
                    if (parent != null) {
                        result.Add(parent);
                        parentId = parent.ParentId;
                    } else {
                        parentId = default(TKey);
                    }
                }
            }
            return result;
        }
        public IList<T> GetParents(TKey id, bool showHidden = false) {
            var item = GetById(id, showHidden);
            return GetParents(item);
        }
        public IList<T> GetChildren(T item, bool showHidden = false) {
            return GetChildren(item.Id, showHidden);
        }
        public IList<T> GetChildren(TKey id, bool showHidden = false) {
            var result = new List<T>();
            GetChildren(result, id, showHidden);
            return result.OrderByDescending(entity => entity.Order).ThenByDescending(entity => entity.CreatedOn).ToList();
        }
        public TreeNode<T> GetBranch(TKey id, bool showHidden = false) {
            var item = GetById(id, showHidden);
            if (item != null) {
                var result = new TreeNode<T>(item);
                GetChildren(result, showHidden);
                return result;
            }
            return null;
        }

        #endregion

        public override IList<T> GetItems(bool showHidden) {
            return _repository.GetItems(entity => (showHidden || !entity.Hidden) && !entity.Deleted)
                .OrderBy(entity => entity.ParentId).ThenByDescending(entity => entity.Order).ThenByDescending(entity => entity.CreatedOn).ToList();
        }
        public override PagedList<T> GetItems(int pageIndex, int pageSize, bool showHidden) {
            var result = _repository.GetItems(entity => (showHidden || !entity.Hidden) && !entity.Deleted)
                .OrderBy(entity => entity.ParentId).ThenByDescending(entity => entity.Order).ThenByDescending(entity => entity.CreatedOn);
            return new PagedList<T>(result.Skip(pageSize * pageIndex).Take(pageSize).ToList(), result.Count);
        }
        public override bool Delete(T item) {
            foreach (var child in GetChildren(item, true)) {
                Delete(child);
            }
            return base.Delete(item);
        }
        public override TKey Show(T item) {
            if (!item.ParentId.Equals(default(TKey))) {
                Show(GetById(item.ParentId, true));
            }
            return base.Show(item);
        }
        public override TKey Hide(T item) {
            foreach (var child in GetChildren(item, true)) {
                Hide(child);
            }
            return base.Hide(item);
        }
        private void GetChildren(IList<T> branch, TKey id, bool showHidden = false) {
            var children = _repository.GetItems(entity => entity.ParentId.Equals(id) && (showHidden || !entity.Hidden) && !entity.Deleted)
                .OrderByDescending(entity => entity.Order).ThenByDescending(entity => entity.CreatedOn).ToList();
            if (children.Count == 0) return;
            foreach (var child in children) {
                branch.Add(child);
                GetChildren(branch, child.Id, showHidden);
            }
        }
        private void GetChildren(TreeNode<T> branch, bool showHidden = false) {
            var children = _repository.GetItems(entity => entity.ParentId.Equals(branch.Item.Id) && (showHidden || !entity.Hidden) && !entity.Deleted)
                .OrderByDescending(entity => entity.Order).ThenByDescending(entity => entity.CreatedOn).ToList();
            if (children.Count == 0) return;
            foreach (var child in children) {
                branch.AddChild(child);
                GetChildren(branch, showHidden);
            }
        }
    }
}