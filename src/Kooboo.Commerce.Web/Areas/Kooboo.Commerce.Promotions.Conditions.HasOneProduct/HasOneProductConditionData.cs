using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Promotions.Conditions.HasOneProduct
{
    public class HasOneProductConditionData
    {
        [Required]
        public string ProductIds { get; set; }

        public HasOneProductConditionData()
        {
        }

        public string Serialize()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static HasOneProductConditionData Deserialize(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return new HasOneProductConditionData();
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<HasOneProductConditionData>(data);
        }
    }
}