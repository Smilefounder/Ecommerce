using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.CmsSite;
using Kooboo.Commerce.API.Products;

namespace Kooboo.Commerce.API
{
    // TODO: Maybe need to seperate CMS plugins and Commerce() extension method?
    //       So I don't need to depend on the assembly containing CMS plugins.
    public static class ProductExtensions
    {
        public static IEnumerable<Product> GetAccessories(this Product product)
        {
            return Site.Current.Commerce().Accessories().ForProduct(product.Id);
        }
    }
}
