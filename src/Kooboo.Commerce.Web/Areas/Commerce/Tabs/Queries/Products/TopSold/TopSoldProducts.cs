using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Web.Mvc.Paging;
using System.ComponentModel;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Web.Framework.UI.Tabs.Queries;

namespace Kooboo.Commerce.Web.Areas.Commerce.Tabs.Queries.Products.TopSold
{
    public class TopSoldProducts : ProductTabQuery<ProductModel>
    {
        public override string Name
        {
            get { return "TopSoldProducts"; }
        }

        public override string DisplayName
        {
            get { return "Top Sold Products"; }
        }

        public override Type ConfigType
        {
            get
            {
                return typeof(TopSoldProductsConfig);
            }
        }

        class SaledProduct
        {
            public int ProductPriceId { get; set; }
            public int ProductId { get; set; }
            public int SaledCount { get; set; }
        }

        public override Pagination Execute(QueryContext context)
        {
            var parameters = context.Config as TopSoldProductsConfig ?? new TopSoldProductsConfig();
            var lastDate = DateTime.Today.AddDays(-1 * parameters.Days);
            var db = context.Instance.Database;

            IQueryable<SaledProduct> orderItemQuery = db.GetRepository<OrderItem>()
                                   .Query()
                                   .GroupBy(o => o.ProductVariantId)
                                   .Select(g => new SaledProduct
                                   {
                                       ProductPriceId = g.FirstOrDefault().ProductVariantId,
                                       ProductId = g.FirstOrDefault().ProductVariant.ProductId,
                                       SaledCount = g.Sum(o => o.Quantity)
                                   })
                                   .OrderByDescending(g => g.SaledCount);

            var total = orderItemQuery.Count();
            orderItemQuery = orderItemQuery.Skip(context.PageSize * context.PageIndex).Take(context.PageSize);

            var productQuery = db.GetRepository<Product>().Query().ByKeywords(context.Keywords);
            
            var saledProduct = orderItemQuery.Join(productQuery,
                                                   saled => saled.ProductId,
                                                   product => product.Id,
                                                   (saled, product) => new { Product = product, SaledCount = saled.SaledCount })
                                             .Select(o => new ProductModel 
                                             {
                                                 Id = o.Product.Id,
                                                 Name = o.Product.Name,
                                                 IsPublished = o.Product.IsPublished,
                                                 ProductTypeId = o.Product.ProductTypeId
                                             });

            var data = saledProduct.ToArray();

            return new Pagination<ProductModel>(data, context.PageIndex, context.PageSize, total);
        }
    }
}
