using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Search.Facets;
using Kooboo.Commerce.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.CMSIntegration
{
    public class ProductSearchResult
    {
        public IList<Product> Products { get; set; }

        public int Total { get; set; }

        public IList<FacetResult> Facets { get; set; }
    }
}