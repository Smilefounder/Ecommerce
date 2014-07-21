using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce
{
    class CommerceRazorViewEngine : RazorViewEngine
    {
        public CommerceRazorViewEngine()
        {
            base.AreaMasterLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/Commerce/Views/Shared/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml", "~/Views/Shared/{0}.cshtml" }; //add: "~/Views/Shared/{0}.cshtml" 
            base.AreaViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/Commerce/Views/Shared/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml", "~/Views/Shared/{0}.cshtml" };//add: "~/Views/Shared/{0}.cshtml"
            base.AreaPartialViewLocationFormats = base.AreaViewLocationFormats;
        }
    }
}