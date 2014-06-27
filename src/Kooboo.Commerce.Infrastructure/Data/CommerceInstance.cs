using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data.Context;
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

        public InstanceMetadata Metadata
        {
            get
            {
                return Database.InstanceMetadata;
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

        public static CommerceInstance Current
        {
            get
            {
                var providers = EngineContext.Current.ResolveAll<ICurrentInstanceProvider>();
                foreach (var provider in providers)
                {
                    var instance = provider.GetCurrentInstance();
                    if (instance != null)
                    {
                        return instance;
                    }
                }

                return null;
            }
        }
    }
}
