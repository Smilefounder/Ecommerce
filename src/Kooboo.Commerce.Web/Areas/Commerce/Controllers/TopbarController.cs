using Kooboo.Commerce.Web.Framework.UI.Topbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class TopbarController : Controller
    {
        public ActionResult CommandConfig(string commandName)
        {
            var command = TopbarCommands.GetCommand(commandName);
            var config = command.GetDefaultConfig() ?? Activator.CreateInstance(command.ConfigType);

            ViewBag.Command = command;

            return PartialView(config);
        }
    }
}
