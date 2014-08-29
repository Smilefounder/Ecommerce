using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public class BehaviorConfig
    {
        public string BehaviorType { get; set; }

        public float Weight { get; set; }

        public static BehaviorConfig Load(string instance, string behaviorType)
        {
            return RecommendationsDataFolder.For(instance)
                                            .GetFolder("Config/Behaviors")
                                            .GetFile(behaviorType + ".config")
                                            .Read<BehaviorConfig>();
        }

        public static void Update(string instance, BehaviorConfig config)
        {
            RecommendationsDataFolder.For(instance)
                                     .GetFolder("Config/Behaviors")
                                     .GetFile(config.BehaviorType + ".config")
                                     .Write(config);
        }
    }
}