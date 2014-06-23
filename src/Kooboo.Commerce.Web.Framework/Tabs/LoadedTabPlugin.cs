using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.Tabs
{
    public class LoadedTabPlugin
    {
        public ITabPlugin TabPlugin { get; private set; }

        public TabLoadContext Context { get; private set; }

        public LoadedTabPlugin(ITabPlugin tabPlugin, TabLoadContext loadContext)
        {
            TabPlugin = tabPlugin;
            Context = loadContext;
        }
    }
}
