using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Scheduling
{
    public interface IJob
    {
        string Id { get; }

        void Execute();
    }
}