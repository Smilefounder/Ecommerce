using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.WebAPI.Controllers
{
    public class ApiInfoController : Controller
    {
        //
        // GET: /ApiInfo/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllAPI()
        {
            return View();
        }

        public ActionResult APIDetail(string id)
        {
            return View();
        }

    }
}
