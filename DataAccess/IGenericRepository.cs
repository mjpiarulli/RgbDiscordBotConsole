using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess
{
    public interface IGenericRepository<T, in TKey> : IDisposable
    {
        bool Add(T entity);
        bool AddBulk(T[] entities);
        bool AddOrUpdate(T entity, Expression<Func<T, bool>> predicate);
        T AddIfNotExists(T entity, Expression<Func<T, bool>> predicate);
        bool Edit(T entity);
        bool Remove(T entity);
        bool RemoveRange(T[] items);
        T Load(TKey id);
        IList<T> LoadAll();
        bool SaveChanges();
        bool Refresh(T entity);
        IQueryable<T> FindBy(Expression<Func<T, bool>> a);
        int Count(Expression<Func<T, bool>> predicate);
        bool Exists(Expression<Func<T, bool>> predicate);
        void ToggleTrackChanges(bool enabled);
        void ExcludeProperty<TProperty>(T entity, Expression<Func<T, TProperty>> expression);
        int ExecuteRawSqlQuery(string sql, object[] @params);
    }
}
