using Kooboo.Commerce.Settings.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Shipping.UPS
{
    public class UPSSettings
    {
        [Required, Display(Name = "Access key")]
        public string AccessKey { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Serialize()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static UPSSettings Deserialize(string json)
        {
            if (String.IsNullOrEmpty(json))
            {
                return new UPSSettings();
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<UPSSettings>(json);
        }

        public void Save(ISettingService service)
        {
            var json = Serialize();
            service.Set(Strings.TrackerName, json, "ShipmentTracking");
        }

        public static UPSSettings LoadFrom(ISettingService service)
        {
            var json = service.Get(Strings.TrackerName, "ShipmentTracking");
            return Deserialize(json);
        }
    }
}