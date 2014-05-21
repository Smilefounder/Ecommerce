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
    public class IDealConfig
    {
        public string PartnerId { get; set; }

        public bool TestMode { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static IDealConfig Deserialize(string data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return new IDealConfig();
            }

            return JsonConvert.DeserializeObject<IDealConfig>(data);
        }
    }
}