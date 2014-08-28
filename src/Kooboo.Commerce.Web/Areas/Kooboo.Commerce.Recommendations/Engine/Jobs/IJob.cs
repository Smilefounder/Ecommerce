using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Jobs
{
    public interface IJob
    {
        void Execute(JobContext context);
    }
}