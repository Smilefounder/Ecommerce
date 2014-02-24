using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Mapping
{
    public class ProductTypeMap : EntityTypeConfiguration<ProductType> {

        public ProductTypeMap() {

            this.HasMany(o => o.CustomFields).WithOptional(o => o.ByCustomFields);

            this.HasMany(o => o.VariationFields).WithOptional(o => o.ByVariationFields);
        }
    }
}
