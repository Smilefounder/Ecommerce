using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Brands;

namespace Kooboo.Commerce.Products.Services
{
    [Dependency(typeof(IProductService))]
    public class ProductService : IProductService
    {
        private readonly ICommerceDatabase _database;

        public ProductService(ICommerceDatabase database)
        {
            _database = database;
        }

        public Product GetById(int id)
        {
            return _database.GetRepository<Product>().Find(id);
        }

        public IQueryable<Product> Query()
        {
            return _database.GetRepository<Product>().Query();
        }

        public IQueryable<ProductVariant> ProductVariants()
        {
            return _database.GetRepository<ProductVariant>().Query();
        }

        public ProductVariant GetProductVariantById(int id)
        {
            return _database.GetRepository<ProductVariant>().Find(id);
        }

        public Product Create(Product product)
        {
            _database.GetRepository<Product>().Insert(product);
            Event.Raise(new ProductCreated(product));

            return product;
        }

        public Product Update(Product product)
        {
            var dbProduct = _database.GetRepository<Product>().Find(product.Id);

            // Basic info
            dbProduct.Name = product.Name;
            dbProduct.Brand = product.BrandId == null ? null : _database.GetRepository<Brand>().Find(product.BrandId.Value);

            dbProduct.UpdateCustomFields(product.CustomFields.ToDictionary(f => f.FieldName, f => f.FieldValue));
            dbProduct.UpdateImages(product.Images);
            dbProduct.UpdateCategories(product.Categories);

            _database.SaveChanges();

            // Product variants
            foreach (var variant in dbProduct.Variants.ToList())
            {
                if (!product.Variants.Any(p => p.Id == variant.Id))
                {
                    RemoveProductVariant(dbProduct, variant.Id, false);
                }
            }

            foreach (var variantModel in product.Variants)
            {
                var current = dbProduct.Variants.FirstOrDefault(it => it.Id == variantModel.Id);
                if (current == null)
                {
                    var variant = new ProductVariant(dbProduct);
                    variant.UpdateFrom(variantModel);
                    AddProductVariant(dbProduct, variant, false);
                }
                else
                {
                    UpdateProductVariant(dbProduct, current.Id, variantModel, false);
                }
            }

            Event.Raise(new ProductUpdated(dbProduct));

            return dbProduct;
        }

        public void Delete(Product product)
        {
            _database.GetRepository<Product>().Delete(product);
            Event.Raise(new ProductDeleted(product));
        }

        public bool Publish(Product product)
        {
            if (product.IsPublished)
            {
                return false;
            }

            product.IsPublished = true;

            _database.SaveChanges();

            Event.Raise(new ProductPublished(product));

            return true;
        }

        public bool Unpublish(Product product)
        {
            if (!product.IsPublished)
            {
                return false;
            }

            product.IsPublished = false;

            _database.SaveChanges();

            Event.Raise(new ProductUnpublished(product));

            return true;
        }

        public void AddProductVariant(Product product, ProductVariant variant, bool notifyProductUpdated)
        {
            product.Variants.Add(variant);
            _database.SaveChanges();

            Event.Raise(new ProductVariantCreated(product, variant));

            if (notifyProductUpdated)
            {
                Event.Raise(new ProductUpdated(product));
            }
        }

        public bool RemoveProductVariant(Product product, int variantId, bool notifyProductUpdated)
        {
            var variant = product.Variants.FirstOrDefault(it => it.Id == variantId);
            if (variant == null)
            {
                return false;
            }

            product.Variants.Remove(variant);
            _database.GetRepository<ProductVariant>().Delete(variant);

            _database.SaveChanges();

            Event.Raise(new ProductVariantDeleted(product, variant));

            if (notifyProductUpdated)
            {
                Event.Raise(new ProductUpdated(product));
            }

            return true;
        }

        public bool UpdateProductVariant(Product product, int variantId, ProductVariant newVariant, bool notifyProductUpdated)
        {
            var variant = product.Variants.FirstOrDefault(it => it.Id == variantId);
            if (variant == null)
            {
                return false;
            }

            variant.UpdateFrom(newVariant);
            _database.SaveChanges();

            Event.Raise(new ProductVariantUpdated(product, variant));

            if (notifyProductUpdated)
            {
                Event.Raise(new ProductUpdated(product));
            }

            return true;
        }
    }
}