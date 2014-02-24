using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class TestController : Controller
    {
        public ICommerceInstanceContext CommerceInstanceContext { get; private set; }

        public TestController(ICommerceInstanceContext context)
        {
            CommerceInstanceContext = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FireOrderPaid()
        {
            var order = new Order
            {
                Id = 1
            };

            Event.Apply(new OrderPaid(order));

            return Content("OK");
        }

        public void Transaction()
        {
            var db = CommerceInstanceContext.CurrentInstance.Database;

            using (var tx = db.BeginTransaction())
            {
                var brand = new Brand
                {
                    Name = "MyBrand 2"
                };

                db.GetRepository<Brand>().Insert(brand);

                tx.Commit();
            }
        }
    }
}
