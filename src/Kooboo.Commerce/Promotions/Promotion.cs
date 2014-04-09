using Kooboo.Commerce.Events;
using Kooboo.Commerce.Events.Promotions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public class Promotion
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public DateTime? StartTimeUtc { get; set; }

        public DateTime? EndTimeUtc { get; set; }

        public bool RequireCouponCode { get; set; }

        [StringLength(20)]
        public string CouponCode { get; set; }

        public bool IsEnabled { get; protected set; }

        public int Priority { get; set; }

        [Required]
        [StringLength(50)]
        public string PromotionPolicyName { get; set; }

        public string PromotionPolicyData { get; set; }

        public string ConditionsExpression { get; set; }

        public PromotionOverlappingUsage OverlappingUsage { get; set; }

        public virtual ICollection<Promotion> OverlappablePromotions { get; protected set; }

        public DateTime CreatedAtUtc { get; protected set; }

        public DateTime LastUpdatedAtUtc { get; protected set; }

        public virtual ICollection<PromotionCustomField> CustomFields { get; protected set; }

        public Promotion()
        {
            CreatedAtUtc = DateTime.UtcNow;
            LastUpdatedAtUtc = DateTime.UtcNow;
            CustomFields = new List<PromotionCustomField>();
            OverlappablePromotions = new List<Promotion>();
        }

        public virtual void MarkUpdated()
        {
            LastUpdatedAtUtc = DateTime.Now;
            Event.Raise(new PromotionUpdated(this));
        }

        public virtual void Enable()
        {
            if (!IsEnabled)
            {
                IsEnabled = true;
                Event.Raise(new PromotionEnabled(this));
            }
        }

        public virtual void Disable()
        {
            if (IsEnabled)
            {
                IsEnabled = false;
                Event.Raise(new PromotionDisabled(this));
            }
        }

        public virtual bool CanBeOverlappedUsedWith(Promotion other)
        {
            if (Priority < other.Priority)
            {
                return other.CanBeOverlappedUsedWith(this);
            }

            if (OverlappingUsage == PromotionOverlappingUsage.NotAllowed)
            {
                return false;
            }
            else if (OverlappingUsage == PromotionOverlappingUsage.AllowedWithAnyPromotion)
            {
                return true;
            }
            else if (OverlappingUsage == PromotionOverlappingUsage.AllowedWithSpecifiedPromotions)
            {
                return OverlappablePromotions.Any(x => x.Id == other.Id);
            }
            else
            {
                throw new NotSupportedException(OverlappingUsage + " is not supported.");
            }
        }
    }
}
