using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Membership;
using Kooboo.CMS.Membership;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.Products;
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
using Kooboo.Commerce.API.Customers;
using System.Globalization;

namespace Kooboo.Commerce.API.LocalProvider.Products
{
    /// <summary>
    /// product api
    /// </summary>
    [Dependency(typeof(IProductAPI))]
    [Dependency(typeof(IProductQuery))]
    public class ProductAPI : LocalCommerceQuery<Product, Kooboo.Commerce.Products.Product>, IProductAPI
    {
        private IProductService _productService;
        private IBrandService _brandService;
        private IProductTypeService _productTypeService;
        private ICategoryService _categoryService;
        private ICustomFieldService _customFieldService;

        public ProductAPI(IProductService productService, IBrandService brandService, IProductTypeService productTypeService, ICustomFieldService customFieldService,
            ICategoryService categoryService,
            IMapper<Product, Kooboo.Commerce.Products.Product> mapper)
            : base(mapper)
        {
            _productService = productService;
            _brandService = brandService;
            _productTypeService = productTypeService;
            _customFieldService = customFieldService;
            _categoryService = categoryService;
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
                        var customer = EngineContext.Current.Resolve<ICustomerAPI>()
                                                    .ByAccountId(member.UUID)
                                                    .FirstOrDefault();
                        if (customer != null)
                        {
                            customerId = customer.Id;
                        }
                    }

                    price.FinalRetailPrice = PriceCalculationContext.GetFinalUnitPrice(product.Id, price.Id, price.RetailPrice, new Kooboo.Commerce.Carts.ShoppingContext
                    {
                        // TODO: We don't need Hal anymore, this need to be changed
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
            EnsureQuery();
            _query = _query.Where(o => o.Id == id);
            return this;
        }

        /// <summary>
        /// add category id filter to query
        /// </summary>
        /// <param name="categoryId">category id</param>
        /// <returns>product query</returns>
        public IProductQuery ByCategoryId(int categoryId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Categories.Any(c => c.CategoryId == categoryId));
            return this;
        }
        /// <summary>
        /// add name filter to query
        /// </summary>
        /// <param name="name">product name</param>
        /// <returns>product query</returns>
        public IProductQuery ByName(string name)
        {
            EnsureQuery();
            name = name.ToLower();
            _query = _query.Where(o => o.Name.ToLower() == name);
            return this;
        }

        /// <summary>
        /// add contains name filter to query
        /// product name contains the input
        /// </summary>
        /// <param name="name">product name</param>
        /// <returns>product query</returns>
        public IProductQuery ContainsName(string name)
        {
            EnsureQuery();
            name = name.ToLower();
            _query = _query.Where(o => o.Name.ToLower().Contains(name));
            return this;
        }

        /// <summary>
        /// add product type id filter to query
        /// </summary>
        /// <param name="productTypeId">product type id</param>
        /// <returns>product query</returns>
        public IProductQuery ByProductTypeId(int productTypeId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.ProductTypeId == productTypeId);
            return this;
        }

        /// <summary>
        /// add brand id filter to query
        /// </summary>
        /// <param name="brandId">product brand id</param>
        /// <returns>product query</returns>
        public IProductQuery ByBrandId(int brandId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.BrandId == brandId);
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
            EnsureQuery();
            _query = _query.Where(o => o.CustomFieldValues.Any(f => f.CustomField.Name == customFieldName && f.FieldValue == fieldValue));
            return this;
        }

        /// <summary>
        /// filter the product by product price variant
        /// </summary>
        /// <param name="variantName">price variant name</param>
        /// <param name="variantVallue">price variant value</param>
        /// <returns>product query</returns>
        public IProductQuery ByPriceVariant(string variantName, string variantValue)
        {
            EnsureQuery();
            _query = _query.Where(o => o.PriceList.Any(p => p.VariantValues.Any(v => v.CustomField.Name == variantName && v.FieldValue == variantValue)));
            return this;
        }
    }
}
