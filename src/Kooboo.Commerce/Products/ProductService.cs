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

namespace Kooboo.Commerce.Products
{
    [Dependency(typeof(ProductService))]
    public class ProductService
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

        public void Create(Product product)
        {
            _database.GetRepository<Product>().Insert(product);
            Event.Raise(new ProductCreated(product));
        }

        public void Update(Product product)
        {
            _database.GetRepository<Product>().Update(product);
            Event.Raise(new ProductUpdated(product));
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
    }
}