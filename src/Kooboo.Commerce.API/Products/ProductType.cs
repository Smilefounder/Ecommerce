using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.API.EAV;

namespace Kooboo.Commerce.API.Products
{
    /// <summary>
    /// Define the type of a product, the type defines all the custom properties and variations. 
    /// </summary>
    public class ProductType {

        public int Id { get; set; }

        public string Name { get; set; }

        public string SkuAlias { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAtUtc { get; set; }

        public CustomField[] CustomFields { get; set; }

        public CustomField[] VariationFields { get; set; }
    }
}
