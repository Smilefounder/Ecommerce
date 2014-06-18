using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Web.Mvc.Paging;

namespace Kooboo.Commerce.Products.ExtendedQuery
{
    [Dependency(typeof(Kooboo.Commerce.ExtendedQuery.ProductQuery), Key = "TopSoldProduct")]
    public class TopSoldProduct : Kooboo.Commerce.ExtendedQuery.ProductQuery
    {
        public string Name
        {
            get { return "TopSoldProduct"; }
        }

        public string Title
        {
            get { return "Top Sold Products"; }
        }

        public string Description
        {
            get { return "Top sold products in the last days"; }
        }

        public ExtendedQueryParameter[] Parameters
        {
            get
            {
                return new ExtendedQueryParameter[]
                    {
                         new ExtendedQueryParameter() { Name = "Num", Description = "top number", Type = typeof(System.Int32), DefaultValue = 10 },
                        new ExtendedQueryParameter() { Name = "Days", Description = "Bought In Days", Type = typeof(System.Int32), DefaultValue = 7 }
                    };
            }
        }

        class SaledProduct
        {
            public int ProductPriceId { get; set; }
            public int ProductId { get; set; }
            public int SaledCount { get; set; }
        }


        public IPagedList<ProductQueryModel> Query(IEnumerable<ExtendedQueryParameter> parameters, ICommerceDatabase db, int pageIndex, int pageSize)
        {
            if (pageIndex <= 1)
                pageIndex = 1;

            int days = 7;
            var para = parameters.FirstOrDefault(o => o.Name == "Days");
            if (para != null && para.Value != null)
                days = Convert.ToInt32(para.Value);
            DateTime lastDate = DateTime.Today.AddDays(-1 * days);

            IQueryable<SaledProduct> orderItemQuery = db.GetRepository<OrderItem>().Query()
                .GroupBy(o => o.ProductPriceId).Select(g => new SaledProduct
                {
                    ProductPriceId = g.FirstOrDefault().ProductPriceId,
                    ProductId = g.FirstOrDefault().ProductPrice.ProductId,
                    SaledCount = g.Sum(o => o.Quantity)
                }).OrderByDescending(g => g.SaledCount);
            var total = orderItemQuery.Count();
            orderItemQuery = orderItemQuery.Skip(pageSize * (pageIndex - 1)).Take(pageSize);


            IQueryable<Product> productQuery = db.GetRepository<Product>().Query();

            var saledProduct = orderItemQuery
                .Join(productQuery,
                           saled => saled.ProductId,
                           product => product.Id,
                           (saled, product) => new { Product = product, SaledCount = saled.SaledCount })
                .Select(o => new ProductQueryModel() { Product = o.Product });

            var data = saledProduct.ToArray();
            return new PagedList<ProductQueryModel>(data, pageIndex, pageSize, total);
        }
    }
}
