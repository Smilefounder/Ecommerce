using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Mapping
{
    class ProductTypeMap : EntityTypeConfiguration<ProductType>
    {
        public ProductTypeMap()
        {
            HasMany(o => o.CustomFields).WithRequired(o => o.ProductType);
            HasMany(o => o.VariationFields).WithRequired(o => o.ProductType);
        }
    }
}
