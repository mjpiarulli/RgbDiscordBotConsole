using System;
using System.Linq.Expressions;

namespace DataAccess.Entity
{
    public interface IGenericEntityRepository<T, TKey> : IGenericRepository<T, TKey> where T : class
    {
        void ToggleTrackChanges(bool enabled);
        void ExcludeProperty<TProperty>(T entity, Expression<Func<T, TProperty>> expression);
        int ExecuteRawSqlQuery(string sql, object[] @params);
        bool Refresh(T entity);
    }
}
