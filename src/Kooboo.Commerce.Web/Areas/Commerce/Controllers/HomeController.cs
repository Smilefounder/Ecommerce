using Kooboo.Commerce.Web.Framework.Mvc;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class HomeController : CommerceController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "CommerceInstance");
        }
    }
}
