using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    // TODO: New name needed, cos it might cause confusion because we also have an IPromotionCondition
    public class PromotionCondition
    {
        public int Id { get; set; }

        public int PromotionId { get; set; }

        [Required]
        public string ConditionName { get; set; }

        public string ConditionData { get; set; }

        [Required]
        public virtual Promotion Promotion { get; set; }

        protected PromotionCondition()
        {
        }

        public PromotionCondition(Promotion promotion, string conditionName)
        {
            Promotion = promotion;
            ConditionName = conditionName;
        }
    }
}
