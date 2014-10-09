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
                return Settings.Name;
            }
        }

        public CommerceInstanceSettings Settings { get; private set; }

        private Func<CommerceInstanceSettings, ICommerceDatabase> _databaseFactory;
        private ICommerceDatabase _database;

        public ICommerceDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = _databaseFactory(Settings);
                }

                return _database;
            }
        }

        public CommerceInstance(CommerceInstanceSettings settings)
            : this(settings, cfg => new CommerceDatabase(cfg))
        {
        }

        public CommerceInstance(CommerceInstanceSettings settings, Func<CommerceInstanceSettings, ICommerceDatabase> databaseFactory)
        {
            Require.NotNull(settings, "settings");
            Require.NotNull(databaseFactory, "databaseFactory");

            Settings = settings;
            _databaseFactory = databaseFactory;
        }

        public void Dispose()
        {
            if (_database != null)
            {
                _database.Dispose();
            }
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
