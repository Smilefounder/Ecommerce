using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Topbar
{
    public interface ITopbarCommand
    {
        string Name { get; }

        string ButtonText { get; }

        string IconClass { get; }

        string ConfirmMessage { get; }

        int Order { get; }

        Type ConfigType { get; }

        IEnumerable<MvcRoute> ApplyTo { get; }

        bool CanExecute(object dataItem, CommerceInstance instance);

        ActionResult Execute(IEnumerable<object> dataItems, object config, CommerceInstance instance);
    }
}
