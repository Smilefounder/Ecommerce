using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.SqlServer
{
    public class SqlServerCommerceDatabaseOperations : ICommerceDatabaseOperations
    {
        public void CreateDatabase(ICommerceDatabase database)
        {
            Require.NotNull(database, "database");

            // TODO: Ugly cast? Maybe better to directly expose entity framework DbContext as a property?
            var dbContext = ((CommerceDatabase)database).DbContext;
            var objectContext = ToObjectContext(dbContext);
            var script = objectContext.CreateDatabaseScript();
            dbContext.Database.ExecuteSqlCommand(script);
        }

        public void DeleteDatabase(ICommerceDatabase database)
        {
            Require.NotNull(database, "database");

            var dbContext = ((CommerceDatabase)database).DbContext;
            var objectContext = ToObjectContext(dbContext);
            var storeItemCollection = (StoreItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.SSpace);
            var script = SqlScripts.DropObjects(storeItemCollection);
            dbContext.Database.ExecuteSqlCommand(script);
        }

        static ObjectContext ToObjectContext(DbContext dbContext)
        {
            return ((IObjectContextAdapter)dbContext).ObjectContext;
        }
    }
}
