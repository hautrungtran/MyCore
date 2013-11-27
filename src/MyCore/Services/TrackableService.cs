using System;
using System.Collections.Generic;
using System.Linq;
using MyCore.Collections;
using MyCore.Data;

namespace MyCore.Services {
    public abstract class TrackableObject<TKey> : UniqueObject<TKey> {
        private DateTime _createdOn;
        protected TrackableObject() {
            _createdOn = DateTime.UtcNow;
        }
        public virtual DateTime CreatedOn {
            get { return _createdOn; }
            set { _createdOn = value; }
        }
        public virtual string CreatedUser { get; set; }
        public virtual DateTime? UpdatedOn { get; set; }
        public virtual string UpdatedUser { get; set; }
    }

    public interface ITrackableService<T, TKey> : IUniqueService<T, TKey> where T : TrackableObject<TKey> {
    }

    public abstract class TrackableService<T, TKey> : UniqueService<T, TKey>, ITrackableService<T, TKey> where T : TrackableObject<TKey> {
        private readonly IRepositoryBase<T, TKey> _repository;
        protected TrackableService(IRepositoryBase<T, TKey> baseRepository)
            : base(baseRepository) {
            _repository = baseRepository;
        }

        #region Overrides of UniqueService<T>

        public override IList<T> GetItems() {
            return _repository.GetItems().OrderByDescending(entity => entity.CreatedOn).ThenByDescending(entity => entity.Id).ToList();
        }
        public override PagedList<T> GetItems(int pageIndex, int pageSize) {
            var items = _repository.GetItems().OrderByDescending(entity => entity.CreatedOn).ThenByDescending(entity => entity.Id);
            return new PagedList<T>(items.Skip(pageSize * pageIndex).Take(pageSize).ToList(), items.Count);
        }
        public override TKey Save(T entity) {
            var now = DateTime.UtcNow;
            if (entity.Id.Equals(default(TKey))) {
                entity.CreatedOn = now;
            }
            entity.UpdatedOn = now;
            return base.Save(entity);
        }

        #endregion
    }
}