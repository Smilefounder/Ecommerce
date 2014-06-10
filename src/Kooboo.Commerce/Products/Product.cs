using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.EAV;
using Kooboo.Commerce.ComponentModel;

namespace Kooboo.Commerce.Products
{
    /// <summary>
    /// This class store the product information, but the sale items. The items to be sold stored in the ProductVariant. 
    /// One product can have multiple variants, each variant contains some field values and some price, stock, etc. 
    /// </summary>
    public class Product : INotifyCreated, INotifyDeleted
    {
        public Product()
        {
            CreatedAtUtc = DateTime.UtcNow;
            IsPublished = false;
            Type = null;
            Brand = null;
            Categories = new List<ProductCategory>();
            Images = new List<ProductImage>();
            CustomFieldValues = new List<ProductCustomFieldValue>();
            PriceList = new List<ProductPrice>();
        }

        public Product(string name, ProductType type) : this()
        {
            Name = name;
            Type = type;
        }

        [Param]
        public int Id { get; set; }

        [Param]
        public string Name { get; set; }

        public int ProductTypeId { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        [Param]
        public bool IsPublished { get; protected set; }

        public DateTime? PublishedAtUtc { get; protected set; }

        [Reference]
        public virtual ProductType Type { get; set; }

        public int? BrandId { get; set; }

        [Reference]
        public virtual Brand Brand { get; set; }

        public virtual ICollection<ProductCategory> Categories { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; }

        public virtual ICollection<ProductCustomFieldValue> CustomFieldValues { get; set; }

        public virtual ICollection<ProductPrice> PriceList { get; set; }

        public virtual ProductPrice FindPrice(int priceId)
        {
            return PriceList.FirstOrDefault(p => p.Id == priceId);
        }

        public virtual ProductPrice CreatePrice(string name, string sku)
        {
            return new ProductPrice(this, name, sku);
        }

        public virtual ProductCustomFieldValue FindCustomFieldValue(int customFieldId)
        {
            return CustomFieldValues.FirstOrDefault(x => x.CustomFieldId == customFieldId);
        }

        public virtual void UpdateCustomFieldValues(IEnumerable<ProductCustomFieldValue> fieldValues)
        {
            UpdateCustomFieldValues(fieldValues.Select(f => new CustomFieldValue(f.CustomFieldId, f.FieldValue)));
        }

        public virtual void UpdateCustomFieldValues(IEnumerable<CustomFieldValue> fieldValues)
        {
            var newFieldValueList = fieldValues.ToList();

            foreach (var fieldValue in CustomFieldValues.ToList())
            {
                if (!newFieldValueList.Any(f => f.FieldId == fieldValue.CustomFieldId))
                {
                    CustomFieldValues.Remove(fieldValue);
                }
            }

            foreach (var fieldValue in newFieldValueList)
            {
                UpdateCustomFieldValue(fieldValue.FieldId, fieldValue.FieldValue);
            }
        }

        public virtual void UpdateCustomFieldValue(int fieldId, string value)
        {
            var fieldValue = FindCustomFieldValue(fieldId);
            if (fieldValue != null)
            {
                fieldValue.FieldValue = value;
            }
            else
            {
                fieldValue = new ProductCustomFieldValue(this, fieldId, value);
                CustomFieldValues.Add(fieldValue);
            }
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
                        ImageSizeName = image.ImageSizeName,
                        ImageUrl = image.ImageUrl,
                        IsVisible = image.IsVisible
                    };
                    Images.Add(current);
                }
                else
                {
                    current.ImageSizeName = image.ImageSizeName;
                    current.ImageUrl = image.ImageUrl;
                    current.IsVisible = image.IsVisible;
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

        public virtual bool MarkPublish()
        {
            if (!IsPublished)
            {
                IsPublished = true;
                PublishedAtUtc = DateTime.UtcNow;
                return true;
            }

            return false;
        }

        public virtual bool MarkUnpublish()
        {
            if (IsPublished)
            {
                IsPublished = false;
                PublishedAtUtc = null;
                return true;
            }

            return false;
        }

        public virtual void NotifyUpdated()
        {
            Event.Raise(new ProductUpdated(this));
        }

        void INotifyCreated.NotifyCreated()
        {
            Event.Raise(new ProductCreated(this));
        }

        void INotifyDeleted.NotifyDeleted()
        {
            Event.Raise(new ProductDeleted(this));
        }
    }
}