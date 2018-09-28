using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using EntityFramework.BulkInsert.Extensions;

namespace DataAccess.Entity
{
    public class GenericEntityRepository<T, TKey> : IGenericRepository<T, TKey> where T : class
    {
        protected readonly DbContext Context;
        private bool _disposed;

        public GenericEntityRepository(DbContext context)
        {
            Context = context;
            Context.Database.CommandTimeout = 300; // 5 * 60
        }

        public virtual bool Add(T entity)
        {
            Context.Set<T>().Add(entity);
            return true;
        }

        public virtual bool AddBulk(T[] entities)
        {
            Context.BulkInsert(entities);
            return true;
        }

        public virtual bool AddOrUpdate(T entity, Expression<Func<T, bool>> predicate)
        {

            var exists = Exists(predicate);
            if (exists)
            {
                Edit(entity);
            }
            else
            {
                Add(entity);
                SaveChanges();
            }

            return true;
        }

        public virtual T AddIfNotExists(T entity, Expression<Func<T, bool>> predicate)
        {
            var exists = Context.Set<T>().FirstOrDefault(predicate);

            if (exists != null)
                return exists;

            Add(entity);
            SaveChanges();
            return entity;
        }

        public virtual bool Edit(T entity)
        {
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            return true;
        }

        public virtual bool Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
            return true;
        }

        public virtual T Load(TKey id) => Context.Set<T>().Find(id);

        public virtual IList<T> LoadAll() => Context.Set<T>().ToList();

        public virtual bool SaveChanges()
        {
            Context.SaveChanges();
            return true;
        }

        public virtual bool Refresh(T entity)
        {
            Context.Entry(entity).Reload();
            return true;
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> q) => Context.Set<T>().Where(q).AsQueryable();

        public int Count(Expression<Func<T, bool>> predicate) => Context.Set<T>().Count(predicate);

        public bool Exists(Expression<Func<T, bool>> predicate) => Context.Set<T>().AsNoTracking().Any(predicate);

        public virtual void ToggleTrackChanges(bool enabled) => Context.Configuration.AutoDetectChangesEnabled = enabled;

        public void ExcludeProperty<TProperty>(T entity, Expression<Func<T, TProperty>> expression)
        {
            Context.Set<T>().Attach(entity);
            Context.Entry(entity).Property(expression).IsModified = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual bool RemoveRange(T[] items)
        {
            Context.Set<T>().RemoveRange(items);

            return true;
        }

        public virtual int ExecuteRawSqlQuery(string sql, object[] @params)
        {
            var rowsAffected = Context.Database.ExecuteSqlCommand(sql, @params);

            return rowsAffected;
        }
    }
}
