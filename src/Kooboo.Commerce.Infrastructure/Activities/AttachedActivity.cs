using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public class AttachedActivity
    {
        public virtual int Id { get; set; }

        [Required, StringLength(100)]
        public virtual string Description { get; set; }

        [Required]
        public virtual string ActivityName { get; set; }

        public virtual string ActivityData { get; set; }

        public virtual bool IsEnabled { get; set; }

        public virtual int Priority { get; set; }

        public virtual DateTime CreatedAtUtc { get; set; }

        public virtual ActivityRule Rule { get; set; }

        public virtual RuleBranch RuleBranch { get; set; }

        protected AttachedActivity()
        {
        }

        public AttachedActivity(ActivityRule rule, RuleBranch branch)
        {
            Rule = rule;
            RuleBranch = branch;
            CreatedAtUtc = DateTime.UtcNow;
        }
    }
}
