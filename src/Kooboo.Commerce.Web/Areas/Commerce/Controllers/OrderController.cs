using System.Linq;
using System.Web.Mvc;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Countries.Services;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.UI.Topbar;
using Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Orders.Default;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class OrderController : CommerceController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = this.CreateTabQueryModel("Orders", new DefaultOrdersQuery());
            return View(model);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var order = _orderService.GetById(id);

            ViewBag.ToolbarCommands = TopbarCommands.GetCommands(ControllerContext, order, CurrentInstance).ToList();
            ViewBag.Return = "/Commerce/Order?siteName=" + Request.QueryString["siteName"] + "&instance=" + Request.QueryString["instance"];
 
            return View(order);
        }
    }
}