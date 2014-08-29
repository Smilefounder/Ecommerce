using Kooboo.Commerce.Data.Folders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Jobs
{
    public class JobConfig
    {
        public string JobName { get; set; }

        public TimeSpan Interval { get; set; }

        public TimeOfDay StartTime { get; set; }

        public static JobConfig Load(string instance, string jobName)
        {
            var file = RecommendationsDataFolder.For(instance).GetFile("Config/Jobs/" + jobName + ".config");
            return file.Read<JobConfig>();
        }

        public static void Update(string instance, JobConfig config)
        {
            if (String.IsNullOrEmpty(config.JobName))
                throw new ArgumentException("config.JobName is required.");

            var file = RecommendationsDataFolder.For(instance).GetFile("Config/Jobs/" + config.JobName + ".config");
            file.Write(config);
        }
    }
}