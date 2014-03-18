using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public class AttachedActivity
    {
        public virtual int Id { get; set; }

        public virtual string Description { get; set; }

        public virtual string ActivityName { get; set; }

        public virtual string ActivityData { get; set; }

        public virtual bool IsEnabled { get; set; }

        public virtual int Priority { get; set; }

        public virtual DateTime CreatedAtUtc { get; set; }

        public virtual ActivityRule Rule { get; set; }

        protected AttachedActivity()
        {
        }

        public AttachedActivity(ActivityRule rule)
        {
            Rule = rule;
            CreatedAtUtc = DateTime.UtcNow;
        }
    }
}
