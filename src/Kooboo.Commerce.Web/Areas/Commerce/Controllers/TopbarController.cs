using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.Mvc.ModelBinding;
using Kooboo.Commerce.Web.Framework.UI.Topbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class TopbarController : CommerceController
    {
        public ActionResult CommandConfig(string commandName)
        {
            var command = TopbarCommands.GetCommand(commandName);
            var config = command.GetDefaultConfig() ?? Activator.CreateInstance(command.ConfigType);

            ViewBag.Command = command;

            return PartialView(config);
        }

        [HttpPost, HandleAjaxFormError, Transactional]
        public ActionResult ExecuteCommand(string commandName, string itemType, CommandDataItem[] model, [ModelBinder(typeof(BindingTypeAwareModelBinder))]object config = null)
        {
            var command = TopbarCommands.GetCommand(commandName);
            var entityType = Type.GetType(itemType, true);
            var repository = CurrentInstance.Database.GetRepository(entityType);
            var idProperty = EntityKey.GetKeyProperty(entityType);
            var items = new List<object>();

            foreach (var each in model)
            {
                var id = Convert.ChangeType(each.Id, idProperty.PropertyType);
                var item = repository.Find(id);
                items.Add(item);
            }

            var result = command.Execute(items, config, CurrentInstance);
            if (result == null)
            {
                result = AjaxForm().ReloadPage();
            }

            return result;
        }

        public class CommandDataItem
        {
            public string Id { get; set; }
        }
    }
}
