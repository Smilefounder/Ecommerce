using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Jobs
{
    public class JobContext
    {
        public string Instance { get; set; }

        public IDictionary<string, string> JobData { get; set; }

        public JobContext(string instance, IDictionary<string, string> jobData)
        {
            Instance = instance;
            JobData = new Dictionary<string, string>(jobData);
        }
    }
}