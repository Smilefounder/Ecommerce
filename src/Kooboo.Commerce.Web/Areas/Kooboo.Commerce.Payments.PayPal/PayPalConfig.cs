using Kooboo.Commerce.Settings.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.PayPal
{
    public class PayPalConfig
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public bool SandboxMode { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static PayPalConfig Deserialize(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return new PayPalConfig();
            }

            return JsonConvert.DeserializeObject<PayPalConfig>(data);
        }
    }
}