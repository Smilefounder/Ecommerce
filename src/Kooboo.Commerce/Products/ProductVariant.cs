using Kooboo.Commerce.EAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products
{
    /// <summary>
    /// The items that is available for cusumer purchase. 
    /// </summary>
    public class ProductVariant
    {
        public int Id { get; set; }

        /// <summary>
        /// The global trade article number. E.g. EAN in Europe. 
        /// </summary>
        public string Sku { get; set; }

        public virtual Product Product { get; set; }

        /*public virtual ICollection<CustomFieldValue> SkuFieldValues { get; set; }*/

        public decimal PurchasePrice { get; set; }

        public decimal RetailPrice { get; set; }

        public int Stock { get; set; }

        public int DeliveryDays { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? PublishedAtUtc { get; set; }
    }
}