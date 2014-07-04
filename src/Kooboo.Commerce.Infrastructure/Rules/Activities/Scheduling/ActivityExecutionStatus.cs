using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Activities.Scheduling
{
    public enum ActivityExecutionStatus
    {
        Pending = 0,
        InProgress = 1,
        Failed = 2,
        Success = 3
    }
}
