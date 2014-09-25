using System.Linq;
using System.Web.Mvc;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Customers;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Countries;
using Kooboo.Commerce.Payments;
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Commerce.Web.Framework.UI.Topbar;
using Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Orders.Default;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class OrderController : CommerceController
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
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
            var order = _orderService.Find(id);

            ViewBag.ToolbarCommands = TopbarCommands.GetCommands(ControllerContext, order, CurrentInstance).ToList();
            ViewBag.Return = "/Commerce/Order?siteName=" + Request.QueryString["siteName"] + "&instance=" + Request.QueryString["instance"];
 
            return View(order);
        }
    }
}