using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Kooboo.Commerce.Settings.Services;

namespace Kooboo.Commerce.Payments.iDeal
{
    public class IDealSettings
    {
        [Required, Display(Name = "Parter ID")]
        public string PartnerId { get; set; }

        [Display(Name = "Test mode")]
        public bool TestMode { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static IDealSettings Deserialize(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return new IDealSettings();
            }

            return JsonConvert.DeserializeObject<IDealSettings>(data);
        }
    }
}