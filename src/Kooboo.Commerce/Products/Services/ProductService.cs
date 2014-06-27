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
        private readonly ICommerceDatabase _db;
        private readonly IRepository<Product> _repoProduct;
        private readonly IRepository<ProductPrice> _repoProductPrice;
        private readonly IRepository<ProductPriceVariantValue> _repoProductPriceVariants;
        private readonly IRepository<CustomField> _repoCustomField;

        public ProductService(ICommerceDatabase db, IRepository<Product> repoProduct, IRepository<ProductPrice> repoProductPrice, IRepository<ProductPriceVariantValue> repoProductPriceVariants, IRepository<CustomField> repoCustomField)
        {
            _db = db;
            _repoProduct = repoProduct;
            _repoProductPrice = repoProductPrice;
            _repoProductPriceVariants = repoProductPriceVariants;
            _repoCustomField = repoCustomField;
        }

        public Product GetById(int id)
        {
            return _repoProduct.Get(o => o.Id == id);
        }

        public IQueryable<Product> Query()
        {
            return _repoProduct.Query();
        }

        public IQueryable<ProductPrice> QueryProductPrices()
        {
            return _repoProductPrice.Query();
        }

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

        public void Create(Product product)
        {
            _repoProduct.Insert(product);
            Event.Raise(new ProductCreated(product));
        }

        public void Delete(Product product)
        {
            _db.GetRepository<Product>().Delete(product);
            Event.Raise(new ProductDeleted(product));
        }

        public bool Publish(Product product)
        {
            if (product.MarkPublish())
            {
                _db.SaveChanges();
                Event.Raise(new ProductPublished(product));
                return true;
            }

            return false;
        }

        public bool Unpublish(Product product)
        {
            if (product.MarkUnpublish())
            {
                _db.SaveChanges();
                Event.Raise(new ProductUnpublished(product));
                return true;
            }

            return false;
        }

        public void AddPrice(Product product, ProductPrice price)
        {
            product.PriceList.Add(price);
            _db.SaveChanges();
            Event.Raise(new ProductVariantAdded(product, price));
        }

        public bool RemovePrice(Product product, int priceId)
        {
            var price = product.FindPrice(priceId);
            if (price == null)
            {
                return false;
            }

            product.PriceList.Remove(price);
            _db.GetRepository<ProductPrice>().Delete(price);
            _db.SaveChanges();

            Event.Raise(new ProductVariantDeleted(product, price));

            return true;
        }

        public bool UpdatePrice(Product product, int priceId, ProductPrice newPrice)
        {
            var price = product.FindPrice(priceId);
            if (price == null)
            {
                return false;
            }

            price.UpdateFrom(newPrice);
            _db.SaveChanges();

            price.NotifyUpdated();

            return true;
        }
    }
}