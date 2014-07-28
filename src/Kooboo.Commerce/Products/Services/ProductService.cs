using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;

namespace Kooboo.Commerce.Products.Services
{
    [Dependency(typeof(IProductService))]
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductVariant> _variantRepository;

        public ProductService(IRepository<Product> productRepository, IRepository<ProductVariant> variantRepository)
        {
            _productRepository = productRepository;
            _variantRepository = variantRepository;
        }

        public Product GetById(int id)
        {
            return _productRepository.Find(id);
        }

        public IQueryable<Product> Query()
        {
            return _productRepository.Query();
        }

        public IQueryable<ProductVariant> ProductVariants()
        {
            return _variantRepository.Query();
        }

        public ProductVariant GetProductVariantById(int id)
        {
            return _variantRepository.Find(id);
        }

        public Product Create(Product product)
        {
            _productRepository.Insert(product);
            Event.Raise(new ProductCreated(product));

            return product;
        }

        public Product Update(Product product)
        {
            var dbProduct = _productRepository.Find(product.Id);

            // Basic info
            dbProduct.Name = product.Name;
            dbProduct.Brand = product.Brand;

            dbProduct.UpdateCustomFields(product.CustomFields.ToDictionary(f => f.FieldName, f => f.FieldValue));
            dbProduct.UpdateImages(product.Images);
            dbProduct.UpdateCategories(product.Categories);

            _productRepository.Database.SaveChanges();

            // Product variants
            foreach (var variant in dbProduct.Variants.ToList())
            {
                if (!product.Variants.Any(p => p.Id == variant.Id))
                {
                    RemovePrice(dbProduct, variant.Id);
                }
            }

            foreach (var variantModel in product.Variants)
            {
                var current = dbProduct.FindVariant(variantModel.Id);
                if (current == null)
                {
                    var variant = dbProduct.CreateVariant(variantModel.Name, variantModel.Sku);
                    variant.UpdateFrom(variantModel);
                    AddPrice(dbProduct, variant);
                }
                else
                {
                    UpdatePrice(dbProduct, current.Id, variantModel);
                }
            }

            Event.Raise(new ProductUpdated(dbProduct));

            return dbProduct;
        }

        public void Delete(Product product)
        {
            _productRepository.Delete(product);
            Event.Raise(new ProductDeleted(product));
        }

        public bool Publish(Product product)
        {
            if (product.IsPublished)
            {
                return false;
            }

            product.IsPublished = true;

            _productRepository.Database.SaveChanges();

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

            _productRepository.Database.SaveChanges();

            Event.Raise(new ProductUnpublished(product));

            return true;
        }

        public void AddPrice(Product product, ProductVariant price)
        {
            product.Variants.Add(price);
            _productRepository.Database.SaveChanges();

            Event.Raise(new ProductVariantAdded(product, price));
        }

        public bool RemovePrice(Product product, int priceId)
        {
            var price = product.FindVariant(priceId);
            if (price == null)
            {
                return false;
            }

            product.Variants.Remove(price);
            _variantRepository.Delete(price);

            _productRepository.Database.SaveChanges();

            Event.Raise(new ProductVariantDeleted(product, price));

            return true;
        }

        public bool UpdatePrice(Product product, int priceId, ProductVariant newPrice)
        {
            var price = product.FindVariant(priceId);
            if (price == null)
            {
                return false;
            }

            price.UpdateFrom(newPrice);
            _productRepository.Database.SaveChanges();

            Event.Raise(new ProductVariantUpdated(product, price));

            return true;
        }
    }
}