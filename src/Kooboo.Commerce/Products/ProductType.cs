using Kooboo.Commerce.EAV;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products {

    /// <summary>
    /// Define the type of a product, the type defines all the custom properties and variations. 
    /// </summary>
    public class ProductType {

        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string SkuAlias { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAtUtc { get; set; }

        public virtual ICollection<ProductTypeCustomField> CustomFields { get; set; }

        public virtual ICollection<ProductTypeVariantField> VariationFields { get; set; }
    }
}
