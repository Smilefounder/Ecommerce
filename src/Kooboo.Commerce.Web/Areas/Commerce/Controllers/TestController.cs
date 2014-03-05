using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Customers.Services;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Orders;
using Kooboo.Commerce.Orders;
using Kooboo.Commerce.Orders.Services;
using Kooboo.Commerce.Payments.Services;
using Kooboo.Commerce.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Web.Mvc;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Payment;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class TestController : Controller
    {
        public CommerceInstanceContext CommerceInstanceContext { get; private set; }

        public TestController(CommerceInstanceContext context)
        {
            CommerceInstanceContext = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Payment()
        {
            var orderService = EngineContext.Current.Resolve<IOrderService>();
            var order = new Order
            {
                Total = 56m
            };
            var customer = EngineContext.Current.Resolve<ICustomerService>().GetAllCustomers(
                null, null, null).First();

            order.Customer = customer;

            orderService.Create(order);

            CommerceInstanceContext.CurrentInstance.Database.SaveChanges();

            var paymentMethod = EngineContext.Current.Resolve<IPaymentMethodService>().Query()
                                             .Where(x => x.PaymentProcessorName == "Fake")
                                             .First();

            var paymentModel = new PaymentRequestModel
            {
                OrderId = order.Id,
                PaymentMethodId = paymentMethod.Id,
                ReturnUrl = Url.Action("PaymentReturn", RouteValues.From(Request.QueryString))
            };

            var routeValues = new RouteValueDictionary(paymentModel);
            routeValues.Merge("commerceName", CommerceInstanceContext.CurrentInstance.Name);

            return RedirectToAction("Gateway", "Payment", routeValues);
        }

        public ActionResult PaymentReturn()
        {
            return Content("Payment OK");
        }

        public ActionResult FireOrderPaid()
        {
            var order = new Order
            {
                Id = 1
            };

            order.MarkPaymentSucceeded(null);

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
