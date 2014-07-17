using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Framework.UI;
using Kooboo.Commerce.Web.Framework.UI.Toolbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Toolbar.Products
{
    public class PublishProductCommand : ProductToolbarCommand
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

        public override bool IsVisible(Kooboo.Commerce.Products.Product product, Data.CommerceInstance instance)
        {
            return !product.IsPublished;
        }

        public override ToolbarCommandResult Execute(Kooboo.Commerce.Products.Product product, object config, Data.CommerceInstance instance)
        {
            var service = EngineContext.Current.Resolve<IProductService>();
            service.Publish(product);
            return null;
        }
    }
}