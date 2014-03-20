using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class CommerceRepository : IRepository
    {
        private CommerceDatabase _database;

        public ICommerceDatabase Database
        {
            get { return _database; }
        }

        public Type EntityType { get; private set; }

        protected CommerceDbContext DbContext
        {
            get
            {
                return _database.DbContext;
            }
        }

        public CommerceRepository(CommerceDatabase database, Type entityType)
        {
            Require.NotNull(database, "database");
            Require.NotNull(entityType, "entityType");

            _database = database;
            EntityType = entityType;
        }

        public object Get(object id)
        {
            return DbContext.Set(EntityType).Find(id);
        }

        public IQueryable Query()
        {
            return DbContext.Set(EntityType);
        }

        public void Insert(object entity)
        {
            var aggregateRoot = entity as AggregateRoot;
            if (aggregateRoot != null)
            {
                aggregateRoot.Metadata = new AggregateMetadata(Database.CommerceInstanceMetadata.Name);
            }

            DbContext.Set(EntityType).Add(entity);
        }

        public void Delete(object entity)
        {
            DbContext.Set(EntityType).Remove(entity);
        }
    }
}
