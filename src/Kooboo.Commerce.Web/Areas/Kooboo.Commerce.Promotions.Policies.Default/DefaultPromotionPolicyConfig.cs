using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Promotions.Policies.Default
{
    public class DefaultPromotionPolicyConfig
    {
        public DiscountAppliedTo DiscountAppliedTo { get; set; }

        public DiscountMode DiscountMode { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal DiscountPercent { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static DefaultPromotionPolicyConfig Deserialize(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return new DefaultPromotionPolicyConfig();
            }

            return JsonConvert.DeserializeObject<DefaultPromotionPolicyConfig>(data);
        }
    }
}