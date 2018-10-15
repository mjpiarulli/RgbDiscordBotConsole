using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace DataAccess.Json
{
    public class GenericJsonFileRepository<T> : IGenericRepository<T, int> 
        where T : class
    {
        private List<T> _objects;
        private string _fullPath;

        public GenericJsonFileRepository()
        {
            var type = typeof(T).Name;
            var path = AppDomain.CurrentDomain.BaseDirectory;
            _fullPath = Path.Combine(path, type) + ".json";
            using (var r = new StreamReader(_fullPath))
            {
                var json = r.ReadToEnd();
                _objects = JsonConvert.DeserializeObject<List<T>>(json);
            }
        }

        public bool Add(T entity)
        {
            _objects.Add(entity);

            return true;
        }

        public bool AddBulk(T[] entities)
        {
            _objects.AddRange(entities);

            return true;
        }

        public T AddIfNotExists(T entity, Expression<Func<T, bool>> predicate)
        {
            if (!Exists(predicate))
                Add(entity);

            return entity;
        }

        public bool AddOrUpdate(T entity, Expression<Func<T, bool>> predicate)
        {
            return Exists(predicate) ? Edit(entity) : Add(entity);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            var count = _objects.Count(predicate.Compile().Invoke);

            return count;
        }

        public void Dispose()
        {
            _objects = null;
            _fullPath = null;
        }

        public bool Edit(T entity)
        {
            var id = _objects.IndexOf(entity);
            _objects[id] = entity;

            return true;
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            var exists = _objects.Exists(predicate.Compile().Invoke);

            return exists;
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> a)
        {
            var queryable = _objects.AsQueryable().Where(a);

            return queryable;
        }

        public T Load(int id)
        {
            var entity = _objects[id];

            return entity;
        }

        public IList<T> LoadAll()
        {
            return _objects;
        }

        public bool Remove(T entity)
        {
            var id = _objects.IndexOf(entity);
            _objects.RemoveAt(id);

            return true;
        }

        public bool RemoveRange(T[] items)
        {
            foreach (var item in items)
                Remove(item);

            return true;
        }

        public bool SaveChanges()
        {
            var json = JsonConvert.SerializeObject(_objects);
            using (var writer = new StreamWriter(_fullPath, false))
            {
                writer.Write(json);
            }

            return true;
        }
    }
}
