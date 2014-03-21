using Kooboo.Commerce.Promotions;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Mapping
{
    public class PromotionMap : EntityTypeConfiguration<Promotion>
    {
        public PromotionMap()
        {
            HasMany(x => x.OverlappablePromotions).WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("PromotionId");
                    m.MapRightKey("OverlappablePromotionId");
                    m.ToTable("Promotion_OverlappablePromotions");
                });
        }
    }
}
