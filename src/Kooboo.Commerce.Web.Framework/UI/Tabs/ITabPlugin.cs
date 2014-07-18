using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Tabs
{
    public interface ITabPlugin
    {
        string Name { get; }

        string DisplayName { get; }

        Type ModelType { get; }

        string VirtualPath { get; }

        int Order { get; }

        IEnumerable<MvcRoute> ApplyTo { get; }

        void OnLoad(TabLoadContext context);

        void OnSubmit(TabSubmitContext context);
    }
}
