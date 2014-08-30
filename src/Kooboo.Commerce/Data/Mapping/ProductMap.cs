using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;

namespace Kooboo.Commerce.Data.Mapping
{
    class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            HasMany(p => p.Images).WithRequired().WillCascadeOnDelete();
            HasMany(p => p.Categories).WithRequired().WillCascadeOnDelete();
            HasMany(p => p.CustomFields).WithRequired().WillCascadeOnDelete();
            HasMany(p => p.Variants).WithRequired().WillCascadeOnDelete();
        }
    }
}