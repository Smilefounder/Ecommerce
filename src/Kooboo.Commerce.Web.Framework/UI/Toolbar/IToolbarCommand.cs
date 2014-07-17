using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Toolbar
{
    public interface IToolbarCommand
    {
        string Name { get; }

        string ButtonText { get; }

        string IconClass { get; }

        string ConfirmMessage { get; }

        int Order { get; }

        Type ConfigType { get; }

        IEnumerable<MvcRoute> ApplyTo { get; }

        bool CanExecute(object data, CommerceInstance instance);

        ToolbarCommandResult Execute(object data, object config, CommerceInstance instance);
    }
}
