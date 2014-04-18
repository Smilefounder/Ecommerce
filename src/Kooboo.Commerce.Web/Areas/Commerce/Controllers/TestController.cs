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
using Kooboo.Commerce.Web.Areas.Commerce.Models.Rules;
using Kooboo.Commerce.HAL;
using Kooboo.Commerce.HAL.Persistence;
using Newtonsoft.Json;
using Kooboo.Commerce.HAL.Serialization;

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

        public void UriResolver()
        {
            var resolver = EngineContext.Current.Resolve<IUriResolver>();
            var resource = resolver.FindResource("/jd/brand/5");
            var test = resource;
        }

        public ActionResult Hal()
        {
            var brand = new Kooboo.Commerce.API.Brands.Brand
            {
                Id = 5,
                Name = "Apple"
            };

            var resourceDescriptorProvider = EngineContext.Current.Resolve<IResourceDescriptorProvider>();
            var resourceLinkPersistence = EngineContext.Current.Resolve<IResourceLinkPersistence>();
            var uriResolver = EngineContext.Current.Resolve<IUriResolver>();

            var wrapper = new HalWrapper(resourceDescriptorProvider, resourceLinkPersistence, uriResolver);
            var controllerContext = new System.Web.Http.Controllers.HttpControllerContext();
            controllerContext.Request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://api.commerce.com/jd/brand/5");

            var resource = wrapper.Wrap("Brand:detail", brand, controllerContext, new { instance = "jd" });

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new Kooboo.Commerce.HAL.Serialization.Converters.ResourceConverter());
            settings.Converters.Add(new Kooboo.Commerce.HAL.Serialization.Converters.LinksConverter());

            var json = JsonConvert.SerializeObject(resource, Formatting.Indented, settings);

            return Content(json);
        }
        public ActionResult HalList()
        {
            var brands = new List<Brand>
            {
                new Brand { Id = 1, Name = "Dell" },
                new Brand { Id = 2, Name = "Microsoft" },
                new Brand { Id = 3, Name = "Lenovo" }
            };

            var resourceDescriptorProvider = EngineContext.Current.Resolve<IResourceDescriptorProvider>();
            var resourceLinkPersistence = EngineContext.Current.Resolve<IResourceLinkPersistence>();
            var uriResolver = EngineContext.Current.Resolve<IUriResolver>();

            var wrapper = new HalWrapper(resourceDescriptorProvider, resourceLinkPersistence, uriResolver);
            var controllerContext = new System.Web.Http.Controllers.HttpControllerContext();
            controllerContext.Request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, "http://api.commerce.com/jd/brands");

            var resource = wrapper.Wrap("Brand:all", brands, controllerContext, new { instance = "jd" }, item =>
            {
                var brand = (Brand)item;
                return new
                {
                    id = brand.Id
                };
            });

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new Kooboo.Commerce.HAL.Serialization.Converters.ResourceConverter());
            settings.Converters.Add(new Kooboo.Commerce.HAL.Serialization.Converters.LinksConverter());

            var json = JsonConvert.SerializeObject(resource, Formatting.Indented, settings);

            return Content(json);
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

        public string Tokenizer()
        {
            var tokenizer = new Tokenizer("CustomerName == ds:customers:\"Mouhong\" and orTotalAmount >= 3.14 or param3 < 25");
            var output = new StringBuilder();
            Token token = null;

            do
            {
                token = tokenizer.NextToken();
                if (token != null)
                {
                    output.Append(token.Kind == TokenKind.StringLiteral ? "\"" + token.Value + "\"" : token.Value).Append("<br/>");
                }

            } while (token != null);

            return output.ToString();
        }

        public string Parser()
        {
            var parser = new Parser();

            try
            {
                var exp = parser.Parse("(CustomerName == ds:customers:\"Mouhong\" or (Age >= 18 or Gender == \"Male\")) and TotalAmount < 59.98");
                return exp.ToString();
            }
            catch (ParserException ex)
            {
                return ex.Message.Replace(Environment.NewLine, "<br/>");
            }
        }
    }
}
