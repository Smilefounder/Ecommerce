using Kooboo.Commerce.Web.Mvc;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class HomeController : CommerceControllerBase
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "CommerceInstance");
        }
    }
}
