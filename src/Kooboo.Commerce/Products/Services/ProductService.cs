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
using Kooboo.Commerce.Categories;

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

        public Product Create(Product model)
        {
            var product = new Product
            {
                Name = model.Name,
                ProductTypeId = model.ProductTypeId
            };

            if (model.BrandId != null)
            {
                product.BrandId = model.BrandId;
                product.Brand = _database.GetRepository<Brand>().Find(model.BrandId.Value);
            }

            if (model.Categories != null)
            {
                foreach (var category in model.Categories)
                {
                    product.Categories.Add(new ProductCategory
                    {
                        Category = _database.GetRepository<Category>().Find(category.CategoryId)
                    });
                }
            }

            if (model.Images != null)
            {
                product.UpdateImages(model.Images);
            }

            product.UpdateCustomFields(model.CustomFields.ToDictionary(f => f.FieldName, f => f.FieldValue));

            _database.SaveChanges();

            foreach (var variant in model.Variants.ToList())
            {
                AddProductVariant(product, variant, false);
            }

            _database.GetRepository<Product>().Insert(product);

            Event.Raise(new ProductCreated(product));

            return product;
        }

        public Product Update(Product model)
        {
            var product = _database.GetRepository<Product>().Find(model.Id);

            // Basic info
            product.Name = model.Name;
            product.Brand = model.BrandId == null ? null : _database.GetRepository<Brand>().Find(model.BrandId.Value);

            product.UpdateCustomFields(model.CustomFields.ToDictionary(f => f.FieldName, f => f.FieldValue));
            product.UpdateImages(model.Images);
            product.UpdateCategories(model.Categories);

            _database.SaveChanges();

            // Product variants
            foreach (var variant in product.Variants.ToList())
            {
                if (!model.Variants.Any(p => p.Id == variant.Id))
                {
                    RemoveProductVariant(product, variant.Id, false);
                }
            }

            foreach (var variantModel in model.Variants)
            {
                var current = product.Variants.FirstOrDefault(it => it.Id == variantModel.Id);
                if (current == null)
                {
                    var variant = new ProductVariant(product);
                    variant.UpdateFrom(variantModel);
                    AddProductVariant(product, variant, false);
                }
                else
                {
                    UpdateProductVariant(product, current.Id, variantModel, false);
                }
            }

            Event.Raise(new ProductUpdated(product));

            return product;
        }

        public void Delete(Product model)
        {
            var product = _database.GetRepository<Product>().Find(model.Id);
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