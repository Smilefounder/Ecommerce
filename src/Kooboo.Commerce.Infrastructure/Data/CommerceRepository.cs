using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Kooboo.Commerce.Data
{
    public class CommerceRepository : IRepository
    {
        private CommerceDatabase _database;

        public CommerceRepository(CommerceDatabase database, Type entityType)
        {
            _database = database;
            EntityType = entityType;
        }

        public ICommerceDatabase Database
        {
            get
            {
                return _database;
            }
        }

        private DbContext DbContext
        {
            get
            {
                return _database.DbContext;
            }
        }

        public Type EntityType { get; private set; }

        public object Find(params object[] ids)
        {
            return DbContext.Set(EntityType).Find(ids);
        }

        public IQueryable Query()
        {
            return DbContext.Set(EntityType);
        }

        public void Insert(object entity)
        {
            DbContext.Set(EntityType).Add(entity);
            DbContext.SaveChanges();
        }

        public void Update(object entity)
        {
            var entry = DbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                var keys = DbContext.GetKeys(entity);
                var dbEntity = DbContext.Set(EntityType).Find(keys);
                Update(dbEntity, entry);
            }
            else
            {
                DbContext.SaveChanges();
            }
        }

        public void Update(object entity, object values)
        {
            var entry = DbContext.Entry(entity);
            entry.CurrentValues.SetValues(values);
            DbContext.SaveChanges();
        }

        public void Delete(object entity)
        {
            DbContext.Set(EntityType).Remove(entity);
            DbContext.SaveChanges();
        }
    }
}