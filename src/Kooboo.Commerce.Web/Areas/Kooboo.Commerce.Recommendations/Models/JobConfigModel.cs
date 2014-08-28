using Kooboo.Commerce.Recommendations.Engine.Jobs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Models
{
    public class JobConfigModel
    {
        public string JobName { get; set; }

        public int Interval { get; set; }

        public int StartHour { get; set; }

        public int StartMinute { get; set; }
    }
}