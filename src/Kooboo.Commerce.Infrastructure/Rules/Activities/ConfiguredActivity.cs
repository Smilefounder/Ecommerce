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

        public ConfiguredActivity(string activityName, string description)
        {
            ActivityName = activityName;
            Description = description;
        }

        public object LoadConfigModel(Type configModelType)
        {
            return LoadConfigModel(Config, configModelType);
        }

        public static object LoadConfigModel(string config, Type configModelType)
        {
            if (String.IsNullOrWhiteSpace(config))
            {
                return null;
            }
            return JsonConvert.DeserializeObject(config, configModelType);
        }
    }
}
