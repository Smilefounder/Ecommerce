using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Toolbar
{
    public static class ToolbarCommands
    {
        public static IToolbarCommand GetCommand(string name)
        {
            return EngineContext.Current
                                .ResolveAll<IToolbarCommand>()
                                .FirstOrDefault(b => b.Name == name);
        }

        public static IEnumerable<IToolbarCommand> GetCommands(ControllerContext controllerContext)
        {
            foreach (var command in EngineContext.Current.ResolveAll<IToolbarCommand>())
            {
                if (command.ApplyTo.Any(x => x.Matches(controllerContext)))
                {
                    yield return command;
                }
            }
        }

        public static IEnumerable<IToolbarCommand> GetCommands(ControllerContext controllerContext, object context, CommerceInstance instance)
        {
            return GetCommands(controllerContext).Where(x => x.CanExecute(context, instance));
        }
    }
}
