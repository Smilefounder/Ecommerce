using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Framework.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Actions.Products
{
    public class PublishProductAction : ProductAction
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

        public override void Execute(Kooboo.Commerce.Products.Product product, Data.CommerceInstance instance)
        {
            var service = EngineContext.Current.Resolve<IProductService>();
            service.Publish(product);
        }
    }
}