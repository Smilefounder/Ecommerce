using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.Commerce.Web.Html
{
    public static class HeadPanelButtonCollectionExtensions
    {
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

        public static HeaderPanelButton AddCreateButton(this HeaderPanelButtonCollection buttons)
        {
            var urlHelper = new UrlHelper(buttons.Html.ViewContext.RequestContext);
            var routeValues = RouteValues.From(buttons.Html.ViewContext.HttpContext.Request.QueryString).Merge("return", buttons.Html.ViewContext.HttpContext.Request.RawUrl);
            return buttons.Add("Create", "add", urlHelper.Action("Create", routeValues));
        }

        public static HeaderPanelButton AddEnableButton(this HeaderPanelButtonCollection buttons)
        {
            return buttons.AddAjaxPostButton("Enable", "add", "Enable")
                          .VisibleWhen(GridChecked.Any)
                          .VisibleWhenSelected(".disabled");
        }

        public static HeaderPanelButton AddDisableButton(this HeaderPanelButtonCollection buttons)
        {
            return buttons.AddAjaxPostButton("Disable", "delete", "Disable")
                          .VisibleWhen(GridChecked.Any)
                          .VisibleWhenSelected(".enabled");
        }

        public static HeaderPanelButton AddDeleteButton(this HeaderPanelButtonCollection buttons)
        {
            return buttons.AddAjaxPostButton("Delete", "delete", "Delete")
                          .VisibleWhen(GridChecked.Any)
                          .WithConfirmMessage("Are you sure to delete this record?");
        }

        public static HeaderPanelButton AddSaveButton(this HeaderPanelButtonCollection buttons)
        {
            return buttons.Add("Save", "save").AsAjaxFormTrigger();
        }

        public static HeaderPanelButton AddSaveAndNextButton(this HeaderPanelButtonCollection buttons)
        {
            return buttons.Add("Next", "next").AsAjaxFormTrigger();
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