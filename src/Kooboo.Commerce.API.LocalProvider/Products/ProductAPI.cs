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

namespace Kooboo.Commerce.API.LocalProvider.Products
{
    /// <summary>
    /// product api
    /// </summary>
    [Dependency(typeof(IProductAPI))]
    [Dependency(typeof(IProductQuery))]
    public class ProductAPI : LocalCommerceQueryAccess<Product, Kooboo.Commerce.Products.Product>, IProductAPI
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

                    price.FinalRetailPrice = PriceCalculationContext.GetFinalUnitPrice(product.Id, price.Id, price.RetailPrice, new Kooboo.Commerce.ShoppingCarts.ShoppingContext
                    {
                        // TODO: We don't need Hal anymore, this need to be changed
                        CustomerId = customerId
                    });
                }
            }

            return product;
        }

        /// <summary>
        /// create object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public override bool Create(Product obj)
        {
            if (obj != null)
            {
                _productService.Create(_mapper.MapFrom(obj));
                return true;
            }
            return false;
        }

        /// <summary>
        /// update object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public override bool Update(Product obj)
        {
            // TODO: Product的关联属性本身是可能没有Include进来的，一次性Update那也许只有EF的Attach可以做，但这样就无法触发详细的事件了
            throw new NotImplementedException();
            //if (obj != null)
            //{
            //    return _productService.Update(_mapper.MapFrom(obj));
            //}
            //return false;
        }

        /// <summary>
        /// create/update object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public override bool Save(Product obj)
        {
            throw new NotImplementedException();
            //if (obj != null)
            //{
            //    return _productService.Save(_mapper.MapFrom(obj));
            //}
            //return false;
        }

        /// <summary>
        /// delete object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public override bool Delete(Product obj)
        {
            if (obj != null)
            {
                var product = _productService.GetById(obj.Id);
                _productService.Delete(product);
                return true;
            }
            return false;
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
        /// add published filter to query
        /// </summary>
        /// <param name="published">product published</param>
        /// <returns>product query</returns>
        public IProductQuery IsPublished(bool published)
        {
            EnsureQuery();
            _query = _query.Where(o => o.IsPublished == published);
            return this;
        }

        /// <summary>
        /// filter the product by custom field value
        /// </summary>
        /// <param name="customFieldId">custom field id</param>
        /// <param name="fieldValue">custom field valule</param>
        /// <returns>product query</returns>
        public IProductQuery ByCustomField(int customFieldId, string fieldValue)
        {
            EnsureQuery();
            _query = _query.Where(o => o.CustomFieldValues.Any(c => c.CustomFieldId == c.CustomFieldId && c.FieldText == fieldValue));
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
        /// <param name="variantId">price variant id</param>
        /// <param name="variantVallue">price variant value</param>
        /// <returns>product query</returns>
        public IProductQuery ByPriceVariant(int variantId, string variantVallue)
        {
            EnsureQuery();
            _query = _query.Where(o => o.PriceList.Any(p => p.VariantValues.Any(c => c.CustomFieldId == c.CustomFieldId && c.FieldText == variantVallue)));
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

        /// <summary>
        /// create product query
        /// </summary>
        /// <returns>product query</returns>
        public IProductQuery Query()
        {
            return this;
        }

        /// <summary>
        /// create product data access
        /// </summary>
        /// <returns>product data access</returns>
        public IProductAccess Access()
        {
            return this;
        }
    }
}
