using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.API.Brands;
using Kooboo.Commerce.API.Products;
using Kooboo.Commerce.Brands.Services;
using Kooboo.Commerce.Categories.Services;
using Kooboo.Commerce.Products.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.LocalProvider.Products
{
    [Dependency(typeof(IProductQuery), ComponentLifeStyle.Transient)]
    public class ProductQuery : LocalCommerceQuery<Product, Kooboo.Commerce.Products.Product>, IProductQuery
    {
        private IProductService _productService;
        private IBrandService _brandService;
        private IProductTypeService _productTypeService;
        private ICategoryService _categoryService;
        private bool _loadWithProductType = false;
        private bool _loadWithBrand = false;
        private bool _loadWithCategories = false;
        private bool _loadWithImages = false;
        private bool _loadWithCustomFields = false;
        private bool _loadWithPriceList = false;

        private IMapper<Brand, Kooboo.Commerce.Brands.Brand> _brandMapper;
        private IMapper<ProductType, Kooboo.Commerce.Products.ProductType> _productTypeMapper;
        private IMapper<ProductPrice, Kooboo.Commerce.Products.ProductPrice> _productPriceMapper;
        private IMapper<ProductImage, Kooboo.Commerce.Products.ProductImage> _productImageMapper;
        private IMapper<ProductCategory, Kooboo.Commerce.Products.ProductCategory> _productCategoryMapper;
        private IMapper<ProductCustomFieldValue, Kooboo.Commerce.Products.ProductCustomFieldValue> _productCustomFieldValueMapper;
        private IMapper<ProductPriceVariantValue, Kooboo.Commerce.Products.ProductPriceVariantValue> _productPriceVariantValueMapper;

        public ProductQuery(IProductService productService, IBrandService brandService, IProductTypeService productTypeService,
            ICategoryService categoryService,
            IMapper<Product, Kooboo.Commerce.Products.Product> mapper,
            IMapper<Brand, Kooboo.Commerce.Brands.Brand> brandMapper,
            IMapper<ProductType, Kooboo.Commerce.Products.ProductType> productTypeMapper,
            IMapper<ProductPrice, Kooboo.Commerce.Products.ProductPrice> productPriceMapper,
            IMapper<ProductImage, Kooboo.Commerce.Products.ProductImage> productImageMapper,
            IMapper<ProductCategory, Kooboo.Commerce.Products.ProductCategory> productCategoryMapper,
            IMapper<ProductCustomFieldValue, Kooboo.Commerce.Products.ProductCustomFieldValue> productCustomFieldValueMapper,
            IMapper<ProductPriceVariantValue, Kooboo.Commerce.Products.ProductPriceVariantValue> productPriceVariantValueMapper)
        {
            _productService = productService;
            _brandService = brandService;
            _productTypeService = productTypeService;
            _categoryService = categoryService;
            _mapper = mapper;
            _brandMapper = brandMapper;
            _productTypeMapper = productTypeMapper;
            _productPriceMapper = productPriceMapper;
            _productImageMapper = productImageMapper;
            _productCategoryMapper = productCategoryMapper;
            _productCustomFieldValueMapper = productCustomFieldValueMapper;
            _productPriceVariantValueMapper = productPriceVariantValueMapper;
        }

        protected override IQueryable<Commerce.Products.Product> CreateQuery()
        {
            return _productService.Query();
        }

        protected override IQueryable<Commerce.Products.Product> OrderByDefault(IQueryable<Commerce.Products.Product> query)
        {
            return query.OrderByDescending(o => o.Id);
        }

        public override bool Create(Product obj)
        {
            if (obj != null)
            {
                return _productService.Create(_mapper.MapFrom(obj));
            }
            return false;
        }

        public override bool Update(Product obj)
        {
            if (obj != null)
            {
                return _productService.Update(_mapper.MapFrom(obj));
            }
            return false;
        }

        public override bool Save(Product obj)
        {
            if (obj != null)
            {
                return _productService.Save(_mapper.MapFrom(obj));
            }
            return false;
        }

        public override bool Delete(Product obj)
        {
            if (obj != null)
            {
                return _productService.Delete(_mapper.MapFrom(obj));
            }
            return false;
        }

        public IProductQuery ById(int id)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Id == id);
            return this;
        }

        public IProductQuery ByCategoryId(int categoryId)
        {
            EnsureQuery();
            _query = _query.Where(o => o.Categories.Any(c => c.CategoryId == categoryId));
            return this;
        }


        public IProductQuery ByName(string name)
        {
            EnsureQuery();
            name = name.ToLower();
            _query = _query.Where(o => o.Name.ToLower() == name);
            return this;
        }

        public IProductQuery ContainsName(string name)
        {
            EnsureQuery();
            name = name.ToLower();
            _query = _query.Where(o => o.Name.ToLower().Contains(name));
            return this;
        }


        public IProductQuery LoadWithProductType()
        {
            _loadWithProductType = true;
            return this;
        }

        public IProductQuery LoadWithBrand()
        {
            _loadWithBrand = true;
            return this;
        }

        public IProductQuery LoadWithCategories()
        {
            _loadWithCategories = true;
            return this;
        }

        public IProductQuery LoadWithImages()
        {
            _loadWithImages = true;
            return this;
        }

        public IProductQuery LoadWithCustomFields()
        {
            _loadWithCustomFields = true;
            return this;
        }

        public IProductQuery LoadWithPriceList()
        {
            _loadWithPriceList = true;
            return this;
        }
        private void LoadWithOptions(Product obj)
        {
            if (_loadWithBrand && obj.BrandId.HasValue)
            {
                var p = _brandService.GetById(obj.BrandId.Value);
                if (p != null)
                    obj.Brand = _brandMapper.MapTo(p);
            }
            if (_loadWithCategories)
            {
                var cates = _productService.ProductCategoryQuery().Where(o => o.ProductId == obj.Id).ToArray();
                obj.Categories = cates.Select(o => _productCategoryMapper.MapTo(o)).ToArray();
            }
            if (_loadWithImages)
            {
                var imgs = _productService.ProductImageQuery().Where(o => o.ProductId == obj.Id).ToArray();
                if (imgs != null)
                {
                    obj.Images = imgs.Select(o => _productImageMapper.MapTo(o)).ToArray();
                }
            }
            if(_loadWithCustomFields)
            {
                var cfields = _productService.ProductCustomFieldQuery().Where(o => o.ProductId == obj.Id).ToArray();
                if (cfields != null)
                    obj.CustomFieldValues = cfields.Select(o => _productCustomFieldValueMapper.MapTo(o)).ToArray();
            }
            if(_loadWithPriceList)
            {
                var prices = _productService.ProductPriceQuery().Where(o => o.ProductId == obj.Id).ToArray();
                if(prices != null)
                {
                    foreach(var price in prices)
                    {
                        price.VariantValues = _productService.ProductPriceVariantQuery().Where(o => o.ProductPriceId == price.Id).ToList();
                    }
                    obj.PriceList = prices.Select(p =>
                        {
                            var np = _productPriceMapper.MapTo(p);
                            np.VariantValues = p.VariantValues.Select(v => _productPriceVariantValueMapper.MapTo(v)).ToArray();
                            return np;
                        }).ToArray();
                }
            }
        }

        private void ResetLoadOptions()
        {
            _loadWithProductType = false;
            _loadWithBrand = false;
            _loadWithCategories = false;
            _loadWithImages = false;
            _loadWithCustomFields = false;
            _loadWithPriceList = false;
        }

        public override Product[] Pagination(int pageIndex, int pageSize)
        {
            var objs = base.Pagination(pageIndex, pageSize);
            foreach (var obj in objs)
            {
                LoadWithOptions(obj);
            }
            ResetLoadOptions();
            return objs;
        }

        public override Product[] ToArray()
        {
            var objs = base.ToArray();
            foreach (var obj in objs)
            {
                LoadWithOptions(obj);
            }
            ResetLoadOptions();
            return objs;
        }

        public override Product FirstOrDefault()
        {
            var obj = base.FirstOrDefault();
            LoadWithOptions(obj);
            ResetLoadOptions();
            return obj;
        }
    }
}
