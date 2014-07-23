using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using System.Web.Routing;
using Kooboo.Commerce.Web.Framework;
using Kooboo.Commerce.Web.Framework.UI.Topbar;
using Kooboo.Commerce.Web.Framework.Mvc;

namespace Kooboo.Commerce.Web.Framework.UI.Html
{
    public static class HeadPanelButtonCollectionExtensions
    {
        public static HeaderPanelButtonCollection AddTopbarCommands(this HeaderPanelButtonCollection buttons, IEnumerable<ITopbarCommand> commands, Type modelType)
        {
            return AddTopbarCommands(buttons, commands, modelType, null);
        }

        public static HeaderPanelButtonCollection AddTopbarCommands(this HeaderPanelButtonCollection buttons, IEnumerable<ITopbarCommand> commands, Type modelType, object modelId)
        {
            if (commands == null)
            {
                return buttons;
            }

            foreach (var cmd in commands)
            {
                buttons.Add(cmd.ButtonText, cmd.IconClass)
                       .VisibleWhenSelected(".cmd-" + cmd.Name)
                       .WithAttributes(new
                       {
                           data_need_config = (cmd.ConfigType != null ? "true" : "false"),
                           data_model_id = modelId,
                           data_model_type = modelType.AssemblyQualifiedName,
                           data_cmd_name = cmd.Name,
                           data_cmd_text = cmd.ButtonText,
                           data_confirm = cmd.ConfirmMessage
                       });
            }

            return buttons;
        }

        public static HeaderPanelButton AddAjaxPostButton(this HeaderPanelButtonCollection buttons, string text, string icon, string action, object additionalRouteValues = null)
        {
            return AddAjaxPostButton(buttons, text, icon, action, additionalRouteValues == null ? new RouteValueDictionary() : new RouteValueDictionary(additionalRouteValues));
        }

        public static HeaderPanelButton AddAjaxPostButton(this HeaderPanelButtonCollection buttons, string text, string icon, string action, RouteValueDictionary additionalRouteValues)
        {
            var urlHelper = new UrlHelper(buttons.Html.ViewContext.RequestContext);
            var url = urlHelper.Action(action, RouteValues.From(buttons.Html.ViewContext.HttpContext.Request.QueryString).Merge(additionalRouteValues));
            return buttons.Add(text, icon, url)
                          .WithCommandType(CommandType.AjaxPost);
        }

        public static HeaderPanelButton AddCreateButton(this HeaderPanelButtonCollection buttons, string actionName = "Create")
        {
            var urlHelper = new UrlHelper(buttons.Html.ViewContext.RequestContext);
            var routeValues = RouteValues.From(buttons.Html.ViewContext.HttpContext.Request.QueryString).Merge("return", buttons.Html.ViewContext.HttpContext.Request.RawUrl);
            return buttons.Add("Create", "add", urlHelper.Action(actionName, routeValues));
        }

        public static HeaderPanelButton AddEnableButton(this HeaderPanelButtonCollection buttons, string actionName = "Enable")
        {
            return buttons.AddAjaxPostButton("Enable", "add", actionName)
                          .VisibleWhen(GridChecked.Any)
                          .VisibleWhenSelected(".disabled");
        }

        public static HeaderPanelButton AddDisableButton(this HeaderPanelButtonCollection buttons, string actionName = "Disable")
        {
            return buttons.AddAjaxPostButton("Disable", "delete", actionName)
                          .VisibleWhen(GridChecked.Any)
                          .VisibleWhenSelected(".enabled");
        }

        public static HeaderPanelButton AddDeleteButton(this HeaderPanelButtonCollection buttons, string actionName = "Delete")
        {
            return buttons.AddAjaxPostButton("Delete", "delete", actionName)
                          .VisibleWhen(GridChecked.Any)
                          .WithConfirmMessage("Are you sure to delete this record?");
        }

        public static HeaderPanelButton AddSaveButton(this HeaderPanelButtonCollection buttons, string ajaxFormId = null)
        {
            return buttons.Add("Save", "save").AsAjaxFormTrigger(ajaxFormId);
        }

        public static HeaderPanelButton AddSaveAndNextButton(this HeaderPanelButtonCollection buttons, string ajaxFormId = null)
        {
            return buttons.Add("Next", "next").AsAjaxFormTrigger(ajaxFormId);
        }

        public static HeaderPanelButton AddCancelButton(this HeaderPanelButtonCollection buttons)
        {
            return buttons.Add("Cancel", "back")
                          .WithUrl(buttons.Html.ViewContext.HttpContext.Request["return"]);
        }

        public static HeaderPanelButton AddBackButton(this HeaderPanelButtonCollection buttons)
        {
            return buttons.Add("Back", "back")
                          .WithUrl(buttons.Html.ViewContext.HttpContext.Request["return"]);
        }
    }
}