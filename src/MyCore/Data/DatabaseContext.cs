using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace MyCore.Data {
    public interface IDatabaseContext : IDisposable {
        IDbSet<T> GetAll<T, TKey>() where T : DomainObject<TKey>;
        T Find<T, TKey>(TKey id) where T : DomainObject<TKey>;
        T Add<T, TKey>(T entity) where T : DomainObject<TKey>;
        T Update<T, TKey>(T entity) where T : DomainObject<TKey>;
        T Remove<T, TKey>(T entity) where T : DomainObject<TKey>;
        int ExcuteCommand(string sql, params object[] parameters);
        IEnumerable<T> Query<T, TKey>(string query, params object[] parameters) where T : DomainObject<TKey>;
        int SaveChanges();
    }
    public class DatabaseContext : DbContext, IDatabaseContext {
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            var registers = ApplicationManager.Current.Factory.ResolveMany<IEntityRegister>();
            foreach (var register in registers) {
                register.RegisterEntities(modelBuilder);
            }
            base.OnModelCreating(modelBuilder);
        }
        public IDbSet<T> GetAll<T, TKey>() where T : DomainObject<TKey> {
            return Set<T>();
        }
        public T Find<T, TKey>(TKey id) where T : DomainObject<TKey> {
            return Set<T>().Find(id);
        }
        public T Add<T, TKey>(T entity) where T : DomainObject<TKey> {
            return Set<T>().Add(entity);
        }
        public T Update<T, TKey>(T entity) where T : DomainObject<TKey> {
            var obj = Set<T>().Find(entity.Id);
            var entry = Entry(entity);
            if (obj == null) {
                Set<T>().Attach(entity);
                entry.State = EntityState.Modified;
            } else if (entry.State == EntityState.Detached) {
                entry = Entry(obj);
                entry.CurrentValues.SetValues(entity);
                entry.State = EntityState.Modified;
            }
            return entity;
        }
        public T Remove<T, TKey>(T entity) where T : DomainObject<TKey> {
            return Set<T>().Remove(entity);
        }
        public int ExcuteCommand(string sql, params object[] parameters) {
            return Database.ExecuteSqlCommand(sql, parameters);
        }
        public IEnumerable<T> Query<T, TKey>(string query, params object[] parameters) where T : DomainObject<TKey> {
            return Database.SqlQuery<T>(query, parameters);
        }
    }
}