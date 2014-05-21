using Kooboo.Commerce.Settings.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.AuthorizeNet
{
    public class AuthorizeNetConfig
    {
        [Display(Name = "Sandbox")]
        public bool SandboxMode { get; set; }

        [Required]
        [Display(Name = "Login ID")]
        public string LoginId { get; set; }

        [Required]
        [Display(Name = "Transaction key")]
        public string TransactionKey { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static AuthorizeNetConfig Deserialize(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return new AuthorizeNetConfig();
            }

            return JsonConvert.DeserializeObject<AuthorizeNetConfig>(data);
        }
    }
}