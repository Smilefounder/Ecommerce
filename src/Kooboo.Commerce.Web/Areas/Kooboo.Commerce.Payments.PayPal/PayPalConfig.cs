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
        [Display(Name = "Client ID")]
        public string ClientId { get; set; }

        [Display(Name = "Client secret")]
        public string ClientSecret { get; set; }

        [Display(Name = "Sandbox mode")]
        public bool SandboxMode { get; set; }
    }
}