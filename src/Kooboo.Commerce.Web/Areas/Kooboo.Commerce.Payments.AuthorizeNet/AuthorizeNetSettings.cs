using Kooboo.Commerce.Settings.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.AuthorizeNet
{
    public class AuthorizeNetSettings
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

        public static AuthorizeNetSettings Deserialize(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<AuthorizeNetSettings>(data);
        }

        public void SaveTo(IKeyValueService service)
        {
            service.Set("Kooboo.Commerce.Payments.AuthorizeNet", Serialize());
        }

        public static AuthorizeNetSettings FetchFrom(IKeyValueService service)
        {
            return Deserialize(service.Get("Kooboo.Commerce.Payments.AuthorizeNet"));
        }
    }
}