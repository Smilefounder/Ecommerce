using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Topbar
{
    public static class TopbarCommands
    {
        public static ITopbarCommand GetCommand(string name)
        {
            return EngineContext.Current
                                .ResolveAll<ITopbarCommand>()
                                .FirstOrDefault(b => b.Name == name);
        }

        public static IEnumerable<ITopbarCommand> GetCommands(ControllerContext controllerContext)
        {
            foreach (var command in EngineContext.Current.ResolveAll<ITopbarCommand>())
            {
                if (command.ApplyTo.Any(x => x.Matches(controllerContext)))
                {
                    yield return command;
                }
            }
        }

        public static IEnumerable<ITopbarCommand> GetCommands(ControllerContext controllerContext, object context, CommerceInstance instance)
        {
            return GetCommands(controllerContext).Where(x => x.CanExecute(context, instance));
        }
    }
}
