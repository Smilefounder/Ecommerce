using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Products.Services;
using Kooboo.Web.Mvc.Paging;
using Kooboo.Commerce.Data;

namespace Kooboo.Commerce.API.RestAPI
{
    [Dependency(typeof(IProductAPI), ComponentLifeStyle.Transient, Key = "RestAPI")]
    public class ProductAPI : RestApiBase, IProductAPI
    {
        public IPagedList<Product> SearchProducts(string userInput, int? categoryId, int pageIndex = 0, int pageSize = 50)
        {
            QueryParameters.Add("userInput", userInput);
            QueryParameters.Add("categoryId", categoryId.HasValue ? categoryId.ToString() : null);
            QueryParameters.Add("pageIndex", pageIndex.ToString());
            QueryParameters.Add("pageSize", pageSize.ToString());
            var data = Get<PageListWrapper<Product>>("Search");
            return data.ToPagedList();
        }

        public Product GetProductById(int id)
        {
            return Get<Product>(id.ToString());
        }

        protected override string ApiControllerPath
        {
            get { return "Product"; }
        }
    }
}
