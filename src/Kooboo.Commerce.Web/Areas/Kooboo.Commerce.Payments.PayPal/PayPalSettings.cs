using Kooboo.Commerce.Settings.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.PayPal
{
    public class PayPalSettings
    {
        [Display(Name = "Client ID")]
        public string ClientId { get; set; }

        [Display(Name = "Client Secret")]
        public string ClientSecret { get; set; }

        [Display(Name = "Sandbox Mode")]
        public bool SandboxMode { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static PayPalSettings Deserialize(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return new PayPalSettings();
            }

            return JsonConvert.DeserializeObject<PayPalSettings>(data);
        }
    }
}