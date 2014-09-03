using System;
using System.Collections.Generic;
using System.Linq;
using Kooboo.Commerce.Brands;
using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Products;
using Kooboo.Commerce.Products;
using Kooboo.Commerce.Globalization;
using Kooboo.Commerce.Carts;
using System.ComponentModel.DataAnnotations;
using Kooboo.Commerce.Categories;

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
            Categories = new List<Category>();
            Images = new List<ProductImage>();
            CustomFields = new List<ProductCustomField>();
            Variants = new List<ProductVariant>();
        }

        [Key, Param]
        public int Id { get; set; }

        [Param, Localizable, StringLength(100)]
        public string Name { get; set; }

        public virtual ProductType ProductType { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        [Param]
        public bool IsPublished { get; set; }

        public DateTime? PublishedAtUtc { get; set; }

        [Reference]
        public virtual Brand Brand { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; }

        public virtual ICollection<ProductCustomField> CustomFields { get; set; }

        public virtual ICollection<ProductVariant> Variants { get; set; }

        public virtual decimal GetFinalPrice(int variantId, ShoppingContext context)
        {
            var variant = Variants.FirstOrDefault(it => it.Id == variantId);
            return variant.GetFinalPrice(context);
        }

        public virtual void SetCustomFields(IDictionary<string, string> fieldValues)
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
                SetCustomField(fieldValue.Key, fieldValue.Value);
            }
        }

        public virtual void SetCustomField(string name, string value)
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

        public virtual void SetImages(IEnumerable<ProductImage> images)
        {
            if (images == null)
            {
                Images.Clear();
            }

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
                        Type = image.Type,
                        ImageUrl = image.ImageUrl
                    };
                    Images.Add(current);
                }
                else
                {
                    current.Type = image.Type;
                    current.ImageUrl = image.ImageUrl;
                }
            }
        }
    }
}