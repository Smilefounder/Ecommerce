using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public class AlwaysRuleModel : RuleModelBase
    {
        public IList<ConfiguredActivityModel> Activities { get; set; }

        public AlwaysRuleModel()
            : base("Always")
        {
            Activities = new List<ConfiguredActivityModel>();
        }
    }
}