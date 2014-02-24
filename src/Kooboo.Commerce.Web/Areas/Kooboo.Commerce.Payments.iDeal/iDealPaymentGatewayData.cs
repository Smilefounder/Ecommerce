using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Kooboo.Commerce.Payments.iDeal
{
    public class IDealPaymentGatewayData
    {
        [Required, Display(Name = "Parter ID")]
        public string PartnerId { get; set; }

        [Display(Name = "Test mode")]
        public bool TestMode { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static IDealPaymentGatewayData Deserialize(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return new IDealPaymentGatewayData();
            }

            return JsonConvert.DeserializeObject<IDealPaymentGatewayData>(data);
        }
    }
}