using System;
using System.Linq;
using System.Reflection;

namespace DataAccess
{
    public abstract class UnitOfWorkBase : IDisposable
    {
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // get all public and private methods that implement IDisposable
                    var disposableFields = GetType().GetFields(BindingFlags.NonPublic |
                                                               BindingFlags.Instance |
                                                               BindingFlags.Public)
                        .Where(f => typeof(IDisposable).IsAssignableFrom(f.FieldType))
                        .ToList();

                    // get the instance of these objects and call the dispose method
                    foreach (var field in disposableFields)
                    {
                        var instance = field.GetValue(this);
                        var dispose = instance.GetType().GetMethod("Dispose");
                        dispose.Invoke(instance, new object[] { });
                    }
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public abstract bool Save();
    }
}
