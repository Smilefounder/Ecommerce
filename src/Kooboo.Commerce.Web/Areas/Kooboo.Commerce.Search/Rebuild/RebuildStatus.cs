using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Rebuild
{
    public enum RebuildStatus
    {
        NotRunning = 0,
        Running = 1,
        Failed = 2,
        Cancelled = 3,
        Success = 4
    }
}