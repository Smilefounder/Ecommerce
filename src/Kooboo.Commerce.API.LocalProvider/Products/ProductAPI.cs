﻿using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.HAL;
using Kooboo.Commerce.API.Products;
using Kooboo.Commerce.Brands.Services;
using Kooboo.Commerce.Categories.Services;
using Kooboo.Commerce.EAV.Services;
using Kooboo.Commerce.Products.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Products
{
    /// <summary>
    /// product api
    /// </summary>
    [Dependency(typeof(IProductAPI), ComponentLifeStyle.Transient)]
    public class ProductAPI : LocalCommerceQueryAccess<Product, Kooboo.Commerce.Products.Product>, IProductAPI
    {
        private IProductService _productService;
        private IBrandService _brandService;
        private IProductTypeService _productTypeService;
        private ICategoryService _categoryService;
        private ICustomFieldService _customFieldService;

        public ProductAPI(IHalWrapper halWrapper, IProductService productService, IBrandService brandService, IProductTypeService productTypeService, ICustomFieldService customFieldService,
            ICategoryService categoryService,
            IMapper<Product, Kooboo.Commerce.Products.Product> mapper)
            : base(halWrapper, mapper)
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

        /// <summary>
        /// create object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public override bool Create(Product obj)
        {
            if (obj != null)
            {
                return _productService.Create(_mapper.MapFrom(obj));
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
            if (obj != null)
            {
                return _productService.Update(_mapper.MapFrom(obj));
            }
            return false;
        }

        /// <summary>
        /// create/update object
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if successfully, else false</returns>
        public override bool Save(Product obj)
        {
            if (obj != null)
            {
                return _productService.Save(_mapper.MapFrom(obj));
            }
            return false;
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
                return _productService.Delete(_mapper.MapFrom(obj));
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
        /// add deleted filter to query
        /// </summary>
        /// <param name="deleted">product deleted</param>
        /// <returns>product query</returns>
        public IProductQuery IsDeleted(bool deleted)
        {
            EnsureQuery();
            _query = _query.Where(o => o.IsDeleted == deleted);
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
            var customFieldQuery = _customFieldService.Query().Where(o => o.Name == customFieldName);
            var customFieldValueQuery = _productService.ProductCustomFieldQuery().Where(o => customFieldQuery.Any(c => c.Id == o.CustomFieldId));
            _query = _query.Where(o => customFieldValueQuery.Any(c => c.ProductId == o.Id));
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
            var customFieldQuery = _customFieldService.Query().Where(o => o.Name == variantName);
            var customFieldValueQuery = _productService.ProductPriceVariantQuery().Where(o => customFieldQuery.Any(c => c.Id == o.CustomFieldId));
            var priceQuery = _productService.ProductPriceQuery().Where(o => o.VariantValues.Any(c => c.ProductPriceId == o.Id));
            _query = _query.Where(o => priceQuery.Any(c => c.ProductId == o.Id));
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
