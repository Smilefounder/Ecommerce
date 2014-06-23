using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.Tabs
{
    public static class TabPlugins
    {
        public static IEnumerable<ITabPlugin> GetTabPlugins(ControllerContext controllerContext)
        {
            var plugins = EngineContext.Current.ResolveAll<ITabPlugin>();
            foreach (var plugin in plugins)
            {
                if (plugin.IsVisible(controllerContext))
                {
                    yield return plugin;
                }
            }
        }

        public static IEnumerable<LoadedTabPlugin> LoadTabPlugins(ControllerContext controllerContext)
        {
            var plugins = GetTabPlugins(controllerContext).ToList();
            var loadedPlugins = new List<LoadedTabPlugin>();

            foreach (var plugin in plugins)
            {
                var context = new TabLoadContext(controllerContext);
                plugin.OnLoad(context);
                loadedPlugins.Add(new LoadedTabPlugin(plugin, context));
            }

            return loadedPlugins;
        }
    }
}
