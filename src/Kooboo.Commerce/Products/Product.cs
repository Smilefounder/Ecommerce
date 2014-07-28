using System;
using System.Collections.Generic;
using System.Linq;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Carts;

namespace Kooboo.Commerce.Products
{
    /// <summary>
    /// This class store the product information, but the sale items. The items to be sold stored in the ProductVariant. 
    /// One product can have multiple variants, each variant contains some field values and some price, stock, etc. 
    /// </summary>
    public class Product : ILocalizable
    {
        public Product()
        {
            CreatedAtUtc = DateTime.UtcNow;
            IsPublished = false;
            Type = null;
            Brand = null;
            Categories = new List<ProductCategory>();
            Images = new List<ProductImage>();
            CustomFields = new List<ProductCustomField>();
            Variants = new List<ProductVariant>();
        }

        public Product(string name, ProductType type)
            : this()
        {
            Name = name;
            Type = type;
        }

        [Param]
        public int Id { get; set; }

        [Param]
        [Localizable]
        public string Name { get; set; }

        public int ProductTypeId { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        [Param]
        public bool IsPublished { get; set; }

        public DateTime? PublishedAtUtc { get; set; }

        [Reference]
        public virtual ProductType Type { get; set; }

        public int? BrandId { get; set; }

        [Reference]
        public virtual Brand Brand { get; set; }

        public virtual ICollection<ProductCategory> Categories { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; }

        public virtual ICollection<ProductCustomField> CustomFields { get; set; }

        public virtual ICollection<ProductVariant> Variants { get; set; }

        public virtual decimal GetFinalPrice(int variantId, ShoppingContext context)
        {
            var variant = FindVariant(variantId);
            return variant.GetFinalPrice(context);
        }

        public virtual ProductVariant FindVariant(int priceId)
        {
            return Variants.FirstOrDefault(p => p.Id == priceId);
        }

        public virtual ProductVariant CreateVariant(string name, string sku)
        {
            return new ProductVariant(this, name, sku);
        }

        public virtual void UpdateCustomFields(IDictionary<string, string> fieldValues)
        {
            foreach (var fieldValue in CustomFields.ToList())
            {
                if (!fieldValues.ContainsKey(fieldValue.FieldName))
                {
                    CustomFields.Remove(fieldValue);
                }
            }

            foreach (var fieldValue in fieldValues)
            {
                UpdateCustomField(fieldValue.Key, fieldValue.Value);
            }
        }

        public virtual void UpdateCustomField(string name, string value)
        {
            var fieldValue = CustomFields.FirstOrDefault(f => f.FieldName == name);
            if (fieldValue != null)
            {
                fieldValue.FieldValue = value;
            }
            else
            {
                fieldValue = new ProductCustomField(name, value);
                CustomFields.Add(fieldValue);
            }
        }

        public virtual ProductImage GetImage(string size)
        {
            return Images.FirstOrDefault(i => i.Size == size);
        }

        public virtual void UpdateImages(IEnumerable<ProductImage> images)
        {
            var newImageList = images.ToList();

            foreach (var image in Images.ToList())
            {
                if (!newImageList.Any(i => i.Id == image.Id))
                {
                    Images.Remove(image);
                }
            }

            foreach (var image in newImageList)
            {
                var current = Images.FirstOrDefault(i => i.Id == image.Id);
                if (current == null)
                {
                    current = new ProductImage
                    {
                        Size = image.Size,
                        ImageUrl = image.ImageUrl
                    };
                    Images.Add(current);
                }
                else
                {
                    current.Size = image.Size;
                    current.ImageUrl = image.ImageUrl;
                }
            }
        }

        public virtual void UpdateCategories(IEnumerable<ProductCategory> categories)
        {
            var newCategoryList = categories.ToList();

            foreach (var category in Categories.ToList())
            {
                if (!newCategoryList.Any(c => c.CategoryId == category.CategoryId))
                {
                    Categories.Remove(category);
                }
            }

            foreach (var category in newCategoryList)
            {
                AddCategory(category.CategoryId);
            }
        }

        public virtual bool AddCategory(int categoryId)
        {
            if (Categories.Any(c => c.CategoryId == categoryId))
            {
                return false;
            }

            Categories.Add(new ProductCategory
            {
                CategoryId = categoryId
            });

            return true;
        }
    }
}