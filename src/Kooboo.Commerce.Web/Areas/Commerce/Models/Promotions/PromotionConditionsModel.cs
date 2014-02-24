using Kooboo.Commerce.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Promotions
{
    public class PromotionConditionsModel
    {
        public int PromotionId { get; set; }

        public string PromotionPolicy { get; set; }

        public IList<PromotionConditionModel> AvailableConditions { get; set; }

        public string SelectedConditionName { get; set; }

        public IList<AddedPromotionConditionModel> AddedConditions { get; set; }

        public PromotionConditionsModel()
        {
            AvailableConditions = new List<PromotionConditionModel>();
            AddedConditions = new List<AddedPromotionConditionModel>();
        }
    }
}