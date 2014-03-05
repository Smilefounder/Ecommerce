using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public class CommerceInstance : IDisposable
    {
        public string Name
        {
            get
            {
                return Metadata.Name;
            }
        }

        public CommerceInstanceMetadata Metadata
        {
            get
            {
                return Database.CommerceInstanceMetadata;
            }
        }

        public ICommerceDatabase Database { get; private set; }

        public CommerceInstance(ICommerceDatabase database)
        {
            Require.NotNull(database, "database");
            Database = database;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
