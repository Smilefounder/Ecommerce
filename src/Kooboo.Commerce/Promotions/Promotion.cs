using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Conditions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace Kooboo.Commerce.Promotions
{
    public class Promotion
    {
        [Param]
        public int Id { get; set; }

        [Param]
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public DateTime? StartTimeUtc { get; set; }

        public DateTime? EndTimeUtc { get; set; }

        public bool RequireCouponCode { get; set; }

        [StringLength(20)]
        public string CouponCode { get; set; }

        public bool IsEnabled { get; set; }

        public int Priority { get; set; }

        [Required]
        [StringLength(50)]
        public string PromotionPolicyName { get; set; }

        private string PromotionPolicyConfig { get; set; }

        public virtual T LoadPolicyConfig<T>()
            where T : class
        {
            return LoadPolicyConfig(typeof(T)) as T;
        }

        public virtual object LoadPolicyConfig(Type configModelType)
        {
            if (String.IsNullOrWhiteSpace(PromotionPolicyConfig))
            {
                return null;
            }

            return JsonConvert.DeserializeObject(PromotionPolicyConfig, configModelType);
        }

        public virtual void UpdatePolicyConfig(object configModel)
        {
            if (configModel == null)
            {
                PromotionPolicyConfig = null;
            }
            else
            {
                PromotionPolicyConfig = JsonConvert.SerializeObject(configModel);
            }
        }

        private string ConditionsJson { get; set; }

        private List<Condition> _conditions;

        [NotMapped]
        public IEnumerable<Condition> Conditions
        {
            get
            {
                if (_conditions == null)
                {
                    if (String.IsNullOrWhiteSpace(ConditionsJson))
                    {
                        _conditions = new List<Condition>();
                    }
                    else
                    {
                        _conditions = JsonConvert.DeserializeObject<List<Condition>>(ConditionsJson);
                    }
                }

                return _conditions;
            }
            set
            {
                if (value == null)
                {
                    _conditions = null;
                    ConditionsJson = null;
                }
                else
                {
                    _conditions = value.ToList();
                    ConditionsJson = JsonConvert.SerializeObject(value);
                }
            }
        }

        public PromotionOverlappingUsage OverlappingUsage { get; set; }

        public virtual ICollection<Promotion> OverlappablePromotions { get; protected set; }

        public DateTime CreatedAtUtc { get; set; }

        public Promotion()
        {
            CreatedAtUtc = DateTime.UtcNow;
            OverlappablePromotions = new List<Promotion>();
        }

        public bool IsAvailableNow()
        {
            return IsAvailableNow(DateTime.UtcNow);
        }

        public bool IsAvailableNow(DateTime utcNow)
        {
            return PromotionSpecifications.AvailableNow(utcNow).Compile()(this);
        }

        public virtual bool RemoveOverlappablePromotion(int promotionId)
        {
            var promotion = OverlappablePromotions.FirstOrDefault(p => p.Id == promotionId);
            if (promotion != null)
            {
                OverlappablePromotions.Remove(promotion);
                return true;
            }

            return false;
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

        #region Entity Mapping

        class PromotionMap : EntityTypeConfiguration<Promotion>
        {
            public PromotionMap()
            {
                Property(c => c.ConditionsJson);
                Property(c => c.PromotionPolicyConfig);

                HasMany(c => c.OverlappablePromotions).WithMany()
                    .Map(m =>
                    {
                        m.MapLeftKey("PromotionId");
                        m.MapRightKey("OverlappablePromotionId");
                        m.ToTable("Promotion_OverlappablePromotions");
                    });
            }
        }

        #endregion
    }
}
