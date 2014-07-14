using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Web.Mvc.Paging;
using System.ComponentModel;
using Kooboo.Commerce.Web.Framework.Queries;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.Web.Queries.Products.TopSold
{
    public class TopSoldProducts : IProductQuery
    {
        public string Name
        {
            get { return "TopSoldProducts"; }
        }

        public string DisplayName
        {
            get { return "Top Sold Products"; }
        }

        public Type ConfigType
        {
            get
            {
                return typeof(TopSoldProductsConfig);
            }
        }

        public Type ResultType
        {
            get
            {
                return typeof(ProductModel);
            }
        }

        class SaledProduct
        {
            public int ProductPriceId { get; set; }
            public int ProductId { get; set; }
            public int SaledCount { get; set; }
        }


        public Pagination Execute(QueryContext context)
        {
            var parameters = context.Config as TopSoldProductsConfig ?? new TopSoldProductsConfig();
            var lastDate = DateTime.Today.AddDays(-1 * parameters.Days);
            var db = context.Instance.Database;

            IQueryable<SaledProduct> orderItemQuery = db.GetRepository<OrderItem>()
                                   .Query()
                                   .GroupBy(o => o.ProductPriceId)
                                   .Select(g => new SaledProduct
                                   {
                                       ProductPriceId = g.FirstOrDefault().ProductPriceId,
                                       ProductId = g.FirstOrDefault().ProductPrice.ProductId,
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
