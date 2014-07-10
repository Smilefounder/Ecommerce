using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Orders;
using Kooboo.Web.Mvc.Paging;
using System.ComponentModel;

namespace Kooboo.Commerce.Products.ExtendedQuery
{
    public class TopSoldProductConfig
    {
        [Description("Top number")]
        public int Num { get; set; }

        [Description("Bought in days")]
        public int Days { get; set; }

        public TopSoldProductConfig()
        {
            Days = 7;
        }
    }

    public class TopSoldProduct : Kooboo.Commerce.IProductExtendedQuery
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

        public Type ConfigModelType
        {
            get
            {
                return typeof(TopSoldProductConfig);
            }
        }

        class SaledProduct
        {
            public int ProductPriceId { get; set; }
            public int ProductId { get; set; }
            public int SaledCount { get; set; }
        }


        public IPagedList<ProductQueryModel> Execute(CommerceInstance instance, int pageIndex, int pageSize, object config)
        {
            if (pageIndex <= 1)
                pageIndex = 1;

            var parameters = config as TopSoldProductConfig ?? new TopSoldProductConfig();

            DateTime lastDate = DateTime.Today.AddDays(-1 * parameters.Days);

            var db = instance.Database;

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
