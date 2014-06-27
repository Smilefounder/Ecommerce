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
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Parsing;
using System.Text;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Conditions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Kooboo.Commerce.API;
using Kooboo.Commerce.Brands.Services;
using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Events.Brands;
using Kooboo.Commerce.Events.Products;
using System.Diagnostics;
using Kooboo.Commerce.ShoppingCarts;
using Kooboo.Commerce.Rules.Parameters;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class TestHandler : IHandle<ProductVariantAdded>
    {
        public void Handle(ProductVariantAdded @event)
        {
            Debug.WriteLine("[" + DateTime.Now + "] " + @event.GetType().Name);
        }
    }

    public class TestController : Controller
    {
        private IBrandService _brandService;
        private IRepository<ActivityQueueItem> _queue;

        public TestController(IBrandService brandService, IRepository<ActivityQueueItem> queue)
        {
            _brandService = brandService;
            _queue = queue;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Params()
        {
            var provider = new DefaultParameterProvider();
            var parameters = provider.GetParameters(typeof(TestContext)).ToList();

            var context = new TestContext
            {
                BrandId = 2465,
                Type = "Normal",
                Ref = new SomeObject {  Prop = 5 }
            };

            foreach (var param in parameters)
            {
                var value = param.ValueResolver.ResolveValue(param, context);
                Response.Write(param.Name + " = " + value);
                Response.Write("<br/>");
            }

            return Content("OK");
        }

        public class TestContext
        {
            [Reference(typeof(Brand))]
            public int BrandId { get; set; }

            [Param]
            public string Type { get; set; }

            [Reference]
            public SomeObject Ref { get; set; }
        }

        public class SomeObject
        {
            [Param]
            public int Prop { get; set; }
        }
    }
}
