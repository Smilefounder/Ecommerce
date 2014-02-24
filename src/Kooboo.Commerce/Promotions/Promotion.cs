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

        public PromotionOverlappingUsage OverlappingUsage { get; set; }

        public virtual ICollection<PromotionCondition> Conditions { get; set; }

        public virtual ICollection<Promotion> OverlappablePromotions { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public Promotion()
        {
            CreatedAtUtc = DateTime.UtcNow;
            Conditions = new List<PromotionCondition>();
            OverlappablePromotions = new List<Promotion>();
        }

        public virtual void Enable()
        {
            if (!IsEnabled)
            {
                IsEnabled = true;
            }
        }

        public virtual void Disable()
        {
            if (IsEnabled)
            {
                IsEnabled = false;
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

        public virtual PromotionCondition FindCondition(int conditionId)
        {
            return Conditions.FirstOrDefault(x => x.Id == conditionId);
        }

        public virtual bool RemoveCondition(int conditionId)
        {
            var condition = FindCondition(conditionId);
            if (condition != null)
            {
                Conditions.Remove(condition);
                return true;
            }

            return false;
        }
    }
}
