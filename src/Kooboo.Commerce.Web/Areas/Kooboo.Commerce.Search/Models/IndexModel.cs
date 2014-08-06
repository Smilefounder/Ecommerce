using Kooboo.Commerce.Search.Rebuild;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.Models
{
    public class IndexModel
    {
        public string Name { get; set; }

        public string DocumentType { get; set; }

        public string Culture { get; set; }

        public RebuildStatus? LastRebuildStatus { get; set; }

        public string LastRebuildError { get; set; }

        public string LastRebuildErrorDetail { get; set; }

        public DateTime? LastSucceededRebuildTimeUtc { get; set; }

        public bool IsRebuilding { get; set; }

        public int RebuildProgress { get; set; }
    }
}