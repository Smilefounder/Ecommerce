using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.Tabs
{
    public interface ITabPlugin
    {
        string Name { get; }

        string DisplayName { get; }

        string VirtualPath { get; }

        int Order { get; }

        bool IsVisible(ControllerContext controllerContext);

        void OnLoad(TabLoadContext context);
    }
}
