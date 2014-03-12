using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Mapping
{
    public class ActivityRuleMap : EntityTypeConfiguration<ActivityRule>
    {
        public ActivityRuleMap()
        {
            HasMany(x => x.AttachedActivities)
                .WithRequired(x => x.Rule)
                .Map(x => x.MapKey("ActivityRule_Id"));
        }
    }

    public class AttachedActivityMap : EntityTypeConfiguration<AttachedActivity>
    {
        public AttachedActivityMap()
        {
        }
    }
}
