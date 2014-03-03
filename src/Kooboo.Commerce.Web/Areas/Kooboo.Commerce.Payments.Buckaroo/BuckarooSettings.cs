using Kooboo.Commerce.Settings.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Payments.Buckaroo
{
    public class BuckarooSettings
    {
        [Required]
        [Display(Name = "Website key")]
        public string WebsiteKey { get; set; }

        [Required]
        [Display(Name = "Secret key")]
        public string SecretKey { get; set; }

        [Display(Name = "Test mode")]
        public bool TestMode { get; set; }

        [Display(Name = "Credit debit mandate reference")]
        public string CreditDebitMandateReference { get; set; }

        [Display(Name = "Credit debit mandate date")]
        public string CreditDebitMandateDate { get; set; }

        public static BuckarooSettings Deserialize(string data)
        {
            if (String.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<BuckarooSettings>(data);
        }

        public string Serialize()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public void SaveTo(IKeyValueService service)
        {
            service.Set("Kooboo.Commerce.Payments.Buckaroo", Serialize());
        }

        public static BuckarooSettings FetchFrom(IKeyValueService service)
        {
            var json = service.Get("Kooboo.Commerce.Payments.Buckaroo");
            return Deserialize(json);
        }
    }
}