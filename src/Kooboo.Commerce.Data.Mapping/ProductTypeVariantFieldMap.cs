using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Mapping
{
    public class ProductTypeVariantFieldMap: EntityTypeConfiguration<ProductTypeVariantField> 
    {

        public ProductTypeVariantFieldMap()
        {

            this.HasRequired(o => o.CustomField);
        }
    }
}
