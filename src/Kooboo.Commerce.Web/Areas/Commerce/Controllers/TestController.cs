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
using Kooboo.Commerce.API.HAL;
using Kooboo.Commerce.API.HAL.Persistence;
using Newtonsoft.Json;
using Kooboo.Commerce.API.HAL.Serialization;
using Newtonsoft.Json.Linq;
using Kooboo.Commerce.API;
using Kooboo.Commerce.Brands.Services;
using Kooboo.Commerce.Activities;
using Kooboo.Commerce.Events.Brands;
using Kooboo.Commerce.Events.Products;
using System.Diagnostics;
using Kooboo.Commerce.ShoppingCarts;

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
        public CommerceInstanceContext CommerceInstanceContext { get; private set; }

        private IBrandService _brandService;
        private IRepository<ActivityQueueItem> _queue;

        public TestController(CommerceInstanceContext context, IBrandService brandService, IRepository<ActivityQueueItem> queue)
        {
            CommerceInstanceContext = context;
            _brandService = brandService;
            _queue = queue;
        }

        public ActionResult Index()
        {
            return View();
        }

        public void Query()
        {
            var cart = CommerceInstanceContext.CurrentInstance.Database
                            .GetRepository<ShoppingCart>()
                            .Query()
                            .Where(c => c.Customer.AccountId == "mouhong@kooboo.com")
                            .FirstOrDefault();

            Response.Write(cart == null ? "NULL" : cart.Id.ToString());
        }

        public void Save()
        {
            var instance = CommerceInstanceContext.CurrentInstance;
            var db = instance.Database;
            //using (var tx = db.BeginTransaction())
            //{
                var brand = db.GetRepository<Brand>().Get(2466);
                brand.Name += " (Hello)";

                db.SaveChanges();

                var brand2 = db.GetRepository<Brand>().Get(2466);
                if (brand2.Name == brand.Name)
                {
                    Response.Write("OK. Can get name " + brand2.Name);
                }

            //    tx.Commit();
            //}
        }

        public ActionResult Params()
        {
            var provider = new DeclaringParameterProvider();
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
