using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyCore.Collections;

namespace MyCore.Data {
    public interface IRepositoryBase<T, TKey> where T : DomainObject<TKey> {
        IEnumerable<T> GetItems();
        IEnumerable<T> GetItems<TOrder>(Expression<Func<T, TOrder>> sortSelector, bool asc = false);
        PagedList<T> GetItems(int pageIndex, int pageSize);
        PagedList<T> GetItems<TOrder>(int pageIndex, int pageSize, Expression<Func<T, TOrder>> sortSelector, bool asc = false);
        IEnumerable<T> GetItems(Expression<Func<T, bool>> searchCondition);
        IEnumerable<T> GetItems<TOrder>(Expression<Func<T, bool>> searchCondition, Expression<Func<T, TOrder>> sortSelector, bool asc = false);
        PagedList<T> GetItems(Expression<Func<T, bool>> searchCondition, int pageIndex, int pageSize);
        PagedList<T> GetItems<TOrder>(Expression<Func<T, bool>> searchCondition, int pageIndex, int pageSize, Expression<Func<T, TOrder>> sortSelector, bool asc = false);
        T GetSingle(TKey id);
        T GetSingle(Expression<Func<T, bool>> func);
        TKey Insert(T entity);
        TKey Update(T entity);
        bool Delete(T entity);
        bool Delete(TKey id);
    }
    public abstract class RepositoryBase<T, TKey> : IRepositoryBase<T, TKey> where T : DomainObject<TKey> {
        private readonly IDatabaseContext _context;
        protected virtual IDatabaseContext Context {
            get {
                return _context;
            }
        }
        private IQueryable<T> _entities;
        protected virtual IQueryable<T> Entities {
            get {
                return _entities ?? (_entities = _context.GetAll<T, TKey>());
            }
        }
        protected RepositoryBase(IDatabaseContext context) {
            _context = context;
            _entities = context.GetAll<T, TKey>();
        }
        public virtual IEnumerable<T> GetItems() {
            return Entities;
        }
        public virtual IEnumerable<T> GetItems<TOrder>(Expression<Func<T, TOrder>> sortSelector, bool asc = false) {
            return asc ? Entities.OrderBy(sortSelector) : Entities.OrderByDescending(sortSelector);
        }
        public virtual PagedList<T> GetItems(int pageIndex, int pageSize) {
            return new PagedList<T>(Entities.Skip(pageIndex * pageSize).Take(pageSize).ToList(), Entities.Count);
        }
        public virtual PagedList<T> GetItems<TOrder>(int pageIndex, int pageSize, Expression<Func<T, TOrder>> sortSelector, bool asc = false) {
            var result = asc ? Entities.OrderBy(sortSelector) : Entities.OrderByDescending(sortSelector);
            return new PagedList<T>(result.Skip(pageIndex * pageSize).Take(pageSize).ToList(), result.Count);
        }
        public virtual IEnumerable<T> GetItems(Expression<Func<T, bool>> func) {
            return Entities.Where(func);
        }
        public virtual IEnumerable<T> GetItems<TOrder>(Expression<Func<T, bool>> func, Expression<Func<T, TOrder>> sortSelector, bool asc = false) {
            var rerult = Entities.Where(func);
            return asc ? rerult.OrderBy(sortSelector) : rerult.OrderByDescending(sortSelector);
        }
        public virtual PagedList<T> GetItems(Expression<Func<T, bool>> func, int pageIndex, int pageSize) {
            var searchResult = Entities.Where(func);
            var result = new PagedList<T>(searchResult.Skip(pageIndex * pageSize).Take(pageSize).ToList(), searchResult.Count);
            return result;
        }
        public virtual PagedList<T> GetItems<TOrder>(Expression<Func<T, bool>> func, int pageIndex, int pageSize, Expression<Func<T, TOrder>> sortSelector, bool asc = false) {
            var searchResult = Entities.Where(func);
            searchResult = asc ? searchResult.OrderBy(sortSelector) : searchResult.OrderByDescending(sortSelector);
            var result = new PagedList<T>(searchResult.Skip(pageIndex * pageSize).Take(pageSize).ToList(), searchResult.Count);
            return result;
        }
        public virtual T GetSingle(TKey id) {
            return _context.Find<T, TKey>(id);
        }
        public virtual T GetSingle(Expression<Func<T, bool>> func) {
            return Entities.Where(func).FirstOrDefault();
        }
        public virtual TKey Insert(T entity) {
            if (entity != null) {
                var result = _context.Add<T, TKey>(entity);
                if (_context.SaveChanges() > 0) {
                    return result.Id;
                }
            }
            return default(TKey);
        }
        public virtual TKey Update(T entity) {
            if (entity != null) {
                var result = _context.Update<T, TKey>(entity);
                if (_context.SaveChanges() > 0) {
                    return result.Id;
                }
            }
            return default(TKey);
        }
        public virtual bool Delete(T entity) {
            if (entity != null) {
                _context.Remove<T, TKey>(entity);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
        public virtual bool Delete(TKey id) {
            var entity = _context.Find<T, TKey>(id);
            return entity != null && Delete(entity);
        }

    }
}