using Kooboo.Commerce.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Tests.Common.Data
{
    public class MockRepository<T> : IRepository<T>
        where T : class
    {
        private int _nextInt32Id = 1;

        private Dictionary<object, T> _items = new Dictionary<object, T>();

        public Dictionary<object, T> Items
        {
            get
            {
                return _items;
            }
        }

        public ICommerceDatabase Database { get; private set; }

        public MockRepository(ICommerceDatabase database)
        {
            Database = database;
        }

        public T Find(params object[] ids)
        {
            if (_items.ContainsKey(ids[0]))
            {
                return _items[ids[0]];
            }

            return null;
        }

        public T Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return Query().FirstOrDefault(predicate);
        }

        public IQueryable<T> Query()
        {
            return _items.Values.AsQueryable();
        }

        public void Create(T entity)
        {
            object key = null;

            var keyProp = EntityKey.GetKeyProperty(typeof(T));
            if (keyProp.PropertyType == typeof(Int32))
            {
                key = _nextInt32Id++;
            }
            else
            {
                key = Guid.NewGuid().ToString();
            }

            keyProp.SetValue(entity, key, null);

            _items.Add(key, entity);
            Database.SaveChanges();
        }

        public void Update(T entity)
        {
            var key = EntityKey.FromEntity(entity).Value;
            if (_items.Remove(key))
            {
                _items.Add(key, entity);
                Database.SaveChanges();
            }
        }

        public void Delete(T entity)
        {
            _items.Remove(EntityKey.FromEntity(entity).Value);
            Database.SaveChanges();
        }
    }
}
