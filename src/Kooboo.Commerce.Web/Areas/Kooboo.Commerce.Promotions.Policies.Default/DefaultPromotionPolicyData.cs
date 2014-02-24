using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Promotions.Policies.Default
{
    public class DefaultPromotionPolicyData
    {
        public DiscountAppliedTo DiscountAppliedTo { get; set; }

        public PriceChangeMode DiscountMode { get; set; }

        public decimal DiscountAmount { get; set; }

        public float DiscountPercent { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static DefaultPromotionPolicyData Deserialize(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return new DefaultPromotionPolicyData();
            }

            return JsonConvert.DeserializeObject<DefaultPromotionPolicyData>(data);
        }
    }
}