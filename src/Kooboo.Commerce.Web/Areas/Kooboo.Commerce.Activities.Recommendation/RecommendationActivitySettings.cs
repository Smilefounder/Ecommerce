using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Activities.Recommendation
{
    public class RecommendationActivitySettings
    {
        [Required, Display(Name = "Api key")]
        public string ApiKey { get; set; }

        [Required, Display(Name = "Api secret")]
        public string ApiSecret { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static RecommendationActivitySettings Deserialize(string data)
        {
            if (String.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<RecommendationActivitySettings>(data);
        }
    }
}