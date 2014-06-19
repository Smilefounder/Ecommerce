using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Activities
{
    public class UpdateActivityParametersRequest
    {
        public int RuleId { get; set; }

        public int AttachedActivityInfoId { get; set; }

        public IDictionary<string, string> Parameters { get; set; }

        public UpdateActivityParametersRequest()
        {
            Parameters = new Dictionary<string, string>();
        }
    }
}