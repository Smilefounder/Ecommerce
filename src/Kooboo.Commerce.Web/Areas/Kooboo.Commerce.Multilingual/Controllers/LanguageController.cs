using Kooboo.Commerce.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Multilingual.Controllers
{
    public class LanguageController : CommerceController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
