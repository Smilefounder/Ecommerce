using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Products.Services;
using Kooboo.Commerce.Web.Framework.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Common.Actions.Products
{
    public class UnpublishProductAction : ProductAction
    {
        public override string Name
        {
            get
            {
                return "UnpublishProduct";
            }
        }

        public override string ButtonText
        {
            get
            {
                return "Unpublish";
            }
        }

        public override void Execute(Kooboo.Commerce.Products.Product product, Data.CommerceInstance instance)
        {
            var serivce = EngineContext.Current.Resolve<IProductService>();
            serivce.Unpublish(product);
        }
    }
}