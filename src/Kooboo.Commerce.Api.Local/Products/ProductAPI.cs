using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Membership;
using Kooboo.CMS.Membership;
using Kooboo.Commerce.Api.Brands;
using Kooboo.Commerce.Brands.Services;
using Kooboo.Commerce.Categories.Services;
using Kooboo.Commerce.EAV.Services;
using Kooboo.Commerce.Orders.Pricing;
using Kooboo.Commerce.Products.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.Api.Customers;
using System.Globalization;
using Kooboo.Commerce.Api.Products;
using Kooboo.Commerce.Api.Local;

namespace Kooboo.Commerce.Api.Local.Products
{
    [Dependency(typeof(IProductApi))]
    [Dependency(typeof(IProductQuery))]
    public class ProductApi : LocalCommerceQuery<Product, Kooboo.Commerce.Products.Product>, IProductApi
    {
        private LocalApiContext _context;
        private IProductService _productService;
        private IBrandService _brandService;
        private IProductTypeService _productTypeService;
        private ICategoryService _categoryService;
        private ICustomFieldService _customFieldService;

        public ProductApi(LocalApiContext context)
        {
            _context = context;
            _productService = _context.ServiceFactory.Products;
            _brandService = _context.ServiceFactory.Brands;
            _productTypeService = _context.ServiceFactory.ProductTypes;
            _customFieldService = _context.ServiceFactory.CustomFields;
            _categoryService = _context.ServiceFactory.Categories;
        }

        /// <summary>
        /// create entity query
        /// </summary>
        /// <returns>queryable object</returns>
        protected override IQueryable<Commerce.Products.Product> CreateQuery()
        {
            return _productService.Query();
        }

        /// <summary>
        /// use the default order when pagination the query
        /// </summary>
        /// <param name="query">pagination query</param>
        /// <returns>ordered query</returns>
        protected override IQueryable<Commerce.Products.Product> OrderByDefault(IQueryable<Commerce.Products.Product> query)
        {
            return query.OrderByDescending(o => o.Id);
        }

        protected override Product Map(Commerce.Products.Product obj)
        {
            var product = base.Map(obj);

            if (product.PriceList != null)
            {
                foreach (var price in product.PriceList)
                {
                    int? customerId = null;

                    // TODO: Ugly!
                    var member = new HttpContextWrapper(HttpContext.Current).Membership().GetMembershipUser();
                    if (member != null)
                    {
                        var customer = EngineContext.Current.Resolve<ICustomerApi>()
                                                    .ByAccountId(member.UUID)
                                                    .FirstOrDefault();
                        if (customer != null)
                        {
                            customerId = customer.Id;
                        }
                    }

                    price.FinalRetailPrice = PriceCalculationContext.GetFinalUnitPrice(product.Id, price.Id, price.RetailPrice, new Kooboo.Commerce.Carts.ShoppingContext
                    {
                        CustomerId = customerId
                    });
                }
            }

            return product;
        }

        /// <summary>
        /// add id filter to query
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns>customer query</returns>
        public IProductQuery ById(int id)
        {
            Query = Query.Where(o => o.Id == id);
            return this;
        }

        /// <summary>
        /// add category id filter to query
        /// </summary>
        /// <param name="categoryId">category id</param>
        /// <returns>product query</returns>
        public IProductQuery ByCategoryId(int categoryId)
        {
            Query = Query.Where(o => o.Categories.Any(c => c.CategoryId == categoryId));
            return this;
        }
        /// <summary>
        /// add name filter to query
        /// </summary>
        /// <param name="name">product name</param>
        /// <returns>product query</returns>
        public IProductQuery ByName(string name)
        {
            Query = Query.Where(o => o.Name == name);
            return this;
        }

        /// <summary>
        /// add product type id filter to query
        /// </summary>
        /// <param name="productTypeId">product type id</param>
        /// <returns>product query</returns>
        public IProductQuery ByProductTypeId(int productTypeId)
        {
            Query = Query.Where(o => o.ProductTypeId == productTypeId);
            return this;
        }

        /// <summary>
        /// add brand id filter to query
        /// </summary>
        /// <param name="brandId">product brand id</param>
        /// <returns>product query</returns>
        public IProductQuery ByBrandId(int brandId)
        {
            Query = Query.Where(o => o.BrandId == brandId);
            return this;
        }

        /// <summary>
        /// filter the product by custom field value
        /// </summary>
        /// <param name="customFieldName">custom field name</param>
        /// <param name="fieldValue">custom field valule</param>
        /// <returns>product query</returns>
        public IProductQuery ByCustomField(string customFieldName, string fieldValue)
        {
            Query = Query.Where(o => o.CustomFieldValues.Any(f => f.CustomField.Name == customFieldName && f.FieldValue == fieldValue));
            return this;
        }

        /// <summary>
        /// filter the product by product price variant
        /// </summary>
        /// <param name="variantName">price variant name</param>
        /// <param name="variantVallue">price variant value</param>
        /// <returns>product query</returns>
        public IProductQuery ByVariantField(string variantName, string variantValue)
        {
            Query = Query.Where(o => o.PriceList.Any(p => p.VariantValues.Any(v => v.CustomField.Name == variantName && v.FieldValue == variantValue)));
            return this;
        }
    }
}
