using Kooboo.Commerce.Promotions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Mapping
{
    public class PromotionConditionMap : EntityTypeConfiguration<PromotionCondition>
    {
        public PromotionConditionMap()
        {
            HasKey(x => new { x.Id, x.PromotionId });
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
