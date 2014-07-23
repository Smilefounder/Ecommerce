using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Framework.UI;
using Kooboo.Commerce.Web.Framework.UI.Topbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Topbar.Products
{
    public class PublishProductCommand : ProductTopbarCommand
    {
        public override string Name
        {
            get
            {
                return "PublishProduct";
            }
        }

        public override string ButtonText
        {
            get
            {
                return "Publish";
            }
        }

        public override IEnumerable<MvcRoute> ApplyTo
        {
            get
            {
                yield return MvcRoutes.Products.List();
            }
        }

        public override bool CanExecute(Kooboo.Commerce.Products.Product product, CommerceInstance instance)
        {
            return !product.IsPublished;
        }

        public override ActionResult Execute(IEnumerable<Product> products, object config, CommerceInstance instance)
        {
            var service = EngineContext.Current.Resolve<IProductService>();
            foreach (var product in products)
            {
                service.Publish(product);
            }

            return null;
        }
    }
}