using Kooboo.Commerce.EAV;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Mapping
{
    public class CustomFieldMap : EntityTypeConfiguration<CustomField>
    {
        public CustomFieldMap()
        {

            this.HasOptional(o => o.ByCustomFields).WithMany(o => o.CustomFields);

            this.HasOptional(o => o.ByVariationFields).WithMany(o => o.VariationFields);

            this.HasMany(o => o.ValidationRules).WithRequired().WillCascadeOnDelete(true);
        }
    }
}
