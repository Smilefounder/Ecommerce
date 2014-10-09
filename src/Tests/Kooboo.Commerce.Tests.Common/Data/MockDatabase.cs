using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Tests.Common.Data
{
    public class MockDatabase : ICommerceDatabase
    {
        private Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public CommerceInstanceSettings InstanceSettings { get; private set; }

        public MockDatabase(CommerceInstanceSettings settings)
        {
            InstanceSettings = settings;
        }

        public IRepository Repository(Type entityType)
        {
            throw new NotImplementedException();
        }

        public MockRepository<T> Repository<T>()
            where T : class
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                _repositories.Add(typeof(T), new MockRepository<T>(this));
            }

            return _repositories[typeof(T)] as MockRepository<T>;
        }

        IRepository<T> ICommerceDatabase.Repository<T>()
        {
            return Repository<T>();
        }

        public ICommerceDbTransaction Transaction { get; private set; }

        public ICommerceDbTransaction BeginTransaction()
        {
            var transaction = new MockDbTransaction();
            Transaction = transaction;
            return transaction;
        }

        public ICommerceDbTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            return BeginTransaction();
        }

        public void Clear()
        {
            _repositories.Clear();
        }

        public void SaveChanges()
        {
        }

        public void Dispose()
        {
        }
    }
}
