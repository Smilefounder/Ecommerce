using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Rebuild
{
    public class RebuildInfo
    {
        public RebuildStatus? LastRebuildStatus { get; set; }

        public string LastRebuildError { get; set; }

        public string LastRebuildErrorDetail { get; set; }

        public DateTime? LastSucceededRebuildTimeUtc { get; set; }

        public void ClearError()
        {
            LastRebuildError = null;
            LastRebuildErrorDetail = null;
        }
    }
}