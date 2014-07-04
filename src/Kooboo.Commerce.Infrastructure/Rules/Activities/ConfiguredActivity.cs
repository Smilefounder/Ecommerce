using Kooboo.Commerce.Activities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Activities
{
    public class ConfiguredActivity
    {
        public string ActivityName { get; set; }

        public string Description { get; set; }

        public string Config { get; set; }

        public bool Async { get; set; }

        public int AsyncDelay { get; set; }

        public object LoadConfigModel(Type configModelType)
        {
            return DeserializeConfigModel(Config, configModelType);
        }

        public static object DeserializeConfigModel(string config, Type configModelType)
        {
            if (String.IsNullOrWhiteSpace(config))
            {
                return null;
            }
            return JsonConvert.DeserializeObject(config, configModelType);
        }
    }
}
