using Kooboo.Commerce.API.HAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Mapping
{
    public class HalRuleResourceMap : EntityTypeConfiguration<HalRuleResource>
    {
        public HalRuleResourceMap()
        {
            HasRequired(o => o.Rule);
        }   
    }
}
