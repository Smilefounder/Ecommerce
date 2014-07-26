﻿using Kooboo.Commerce.Api.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Products
{
    /// <summary>
    /// product price
    /// </summary>
    public class ProductPrice
    {
        /// <summary>
        /// price id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// product id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// product price name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// product sku
        /// </summary>
        public string Sku { get; set; }
        /// <summary>
        /// purchase price
        /// </summary>
        public decimal PurchasePrice { get; set; }
        /// <summary>
        /// sale price in retail
        /// </summary>
        public decimal RetailPrice { get; set; }

        public decimal FinalRetailPrice { get; set; }
        /// <summary>
        /// create time at utc
        /// </summary>
        public DateTime CreatedAtUtc { get; set; }
        /// <summary>
        /// product
        /// </summary>
        public Product Product { get; set; }
        
        public ICollection<CustomFieldValue> VariantFields { get; set; }

        public ProductPrice()
        {
            VariantFields = new List<CustomFieldValue>();
        }

        public CustomFieldValue GetVariantField(string name)
        {
            return VariantFields.FirstOrDefault(f => f.FieldName == name);
        }
    }
}
