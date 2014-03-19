using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.EAV;

namespace Kooboo.Commerce.Products.Services
{
    [Dependency(typeof(IProductService))]
    public class ProductService : IProductService
    {
        private readonly ICommerceDatabase _db;
        private readonly IRepository<Product> _repoProduct;
        private readonly IRepository<ProductCategory> _repoProductCategory;
        private readonly IRepository<ProductCustomFieldValue> _repoProductCustomFields;
        private readonly IRepository<ProductImage> _repoProductImage;
        private readonly IRepository<ProductPrice> _repoProductPrice;
        private readonly IRepository<ProductPriceVariantValue> _repoProductPriceVariants;
        private readonly IRepository<CustomField> _repoCustomField;

        public ProductService(ICommerceDatabase db, IRepository<Product> repoProduct, IRepository<ProductCategory> repoProductCategory, IRepository<ProductCustomFieldValue> repoProductCustomFields, IRepository<ProductImage> repoProductImage, IRepository<ProductPrice> repoProductPrice, IRepository<ProductPriceVariantValue> repoProductPriceVariants, IRepository<CustomField> repoCustomField)
        {
            _db = db;
            _repoProduct = repoProduct;
            _repoProductCategory = repoProductCategory;
            _repoProductCustomFields = repoProductCustomFields;
            _repoProductImage = repoProductImage;
            _repoProductPrice = repoProductPrice;
            _repoProductPriceVariants = repoProductPriceVariants;
            _repoCustomField = repoCustomField;
        }

        public Product GetById(int id)
        {
            Product product = _repoProduct.Get(o => o.Id == id);
            return product.IsDeleted ? null : product;
        }

        public IQueryable<Product> Query()
        {
            return _repoProduct.Query();
        }

        public IQueryable<ProductPrice> ProductPriceQuery()
        {
            return _repoProductPrice.Query();
        }

        public IQueryable<ProductCategory> ProductCategoryQuery()
        {
            return _repoProductCategory.Query();
        }

        public IQueryable<ProductImage> ProductImageQuery()
        {
            return _repoProductImage.Query();
        }

        public IQueryable<ProductCustomFieldValue> ProductCustomFieldQuery()
        {
            return _repoProductCustomFields.Query();
        }

        public IQueryable<ProductPriceVariantValue> ProductPriceVariantQuery()
        {
            return _repoProductPriceVariants.Query();
        }


        //public IPagedList<Product> GetAllProducts(string userInput, int? categoryId, int? pageIndex, int? pageSize)
        //{
        //    var query = _repoProduct.Query(o => o.IsDeleted == false);
        //    if (!string.IsNullOrEmpty(userInput))
        //        query = query.Where(o => o.Name.StartsWith(userInput));
        //    if (categoryId.HasValue)
        //        query = query.Where(o => o.Categories.Any(c => c.CategoryId == categoryId.Value));
        //    query = query.OrderBy(o => o.Id);
        //    return PageLinqExtensions.ToPagedList(query, pageIndex ?? 1, pageSize ?? 50);
        //}

        //public IPagedList<ProductPrice> GetAllProductPrices(int? pageIndex, int? pageSize)
        //{
        //    var query = _repoProductPrice.Query().OrderBy(o => o.Id);
        //    return PageLinqExtensions.ToPagedList(query, pageIndex ?? 1, pageSize ?? 50);
        //}

        public ProductPrice GetProductPriceById(int id, bool loadProduct = true, bool loadVariants = true, bool loadCustomFields = true)
        {
            var price = _repoProductPrice.Query(o => o.Id == id).FirstOrDefault();
            if (price != null)
            {
                price.Product = _repoProduct.Query(o => o.Id == price.ProductId).First();
                price.VariantValues = _repoProductPriceVariants.Query(o => o.ProductPriceId == price.Id).ToList();
                foreach (var v in price.VariantValues)
                {
                    v.CustomField = _repoCustomField.Query(o => o.Id == v.CustomFieldId).First();
                }
            }
            return price;
        }

        public bool Create(Product product)
        {
            return _repoProduct.Insert(product);
        }

        public bool Update(Product product)
        {
            try
            {
                using (var tx = _db.BeginTransaction())
                {
                    _repoProduct.Save(o => o.Id == product.Id, product, o => new object[] { o.Id });

                    var dbProductCategories = _repoProductCategory.Query(o => o.ProductId == product.Id).ToArray();
                    _repoProductCategory.SaveAll(_db, dbProductCategories, product.Categories, o => new object[] { o.ProductId, o.CategoryId }, (o, n) => o.ProductId == n.ProductId && o.CategoryId == n.CategoryId);

                    var dbProductImages = _repoProductImage.Query(o => o.ProductId == product.Id).ToArray();
                    _repoProductImage.SaveAll(_db, dbProductImages, product.Images, o => new object[] { o.Id }, (o, n) => o.ProductId == n.ProductId && o.ImageSizeName == n.ImageSizeName);

                    var dbCustomFieldValues = _repoProductCustomFields.Query(o => o.ProductId == product.Id).ToArray();
                    _repoProductCustomFields.SaveAll(_db, dbCustomFieldValues, product.CustomFieldValues, o => new object[] { o.ProductId, o.CustomFieldId }, (o, n) => o.ProductId == n.ProductId && o.CustomFieldId == n.CustomFieldId);

                    var dbProductPrice = _repoProductPrice.Query(o => o.ProductId == product.Id).ToArray();
                    _repoProductPrice.SaveAll(_db, dbProductPrice, product.PriceList, (o, n) => o.Id == n.Id,
                        (repo, o) => repo.Insert(o),
                        (repo, o, n) =>
                        {
                            var dbPriceVariants = _repoProductPriceVariants.Query(v => v.ProductPriceId == o.Id).ToArray();
                            _repoProductPriceVariants.SaveAll(_db, dbPriceVariants, n.VariantValues, k => new object[] { k.ProductPriceId, k.CustomFieldId }, (vo, vn) => vo.ProductPriceId == vn.ProductPriceId && vo.CustomFieldId == vn.CustomFieldId);
                            repo.Update(n, k => new object[] { k.Id });
                        },
                        (repo, o) => repo.Delete(o));

                    tx.Commit();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Save(Product product)
        {
            if (product.Id > 0)
            {
                bool exists = _repoProduct.Query(o => o.Id == product.Id).Any();
                if (exists)
                    return Update(product);
                else
                    return Create(product);
            }
            else
            {
                return Create(product);
            }
        }

        public bool Delete(Product product)
        {
            product.IsDeleted = true;
            if (!product.DeletedAtUtc.HasValue)
            {
                product.DeletedAtUtc = DateTime.UtcNow;
            }
            return _repoProduct.Update(product, k => new object[] { k.Id });
        }

        public void Publish(Product product)
        {
            product.IsPublished = true;
            if (!product.PublishedAtUtc.HasValue)
            {
                product.PublishedAtUtc = DateTime.UtcNow;
            }
            _repoProduct.Update(product, k => new object[] { k.Id });
        }

        public void Unpublish(Product product)
        {
            product.IsPublished = false;
            if (product.PublishedAtUtc.HasValue)
            {
                product.PublishedAtUtc = null;
            }
            _repoProduct.Update(product, k => new object[] { k.Id });
        }
    }
}