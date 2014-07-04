using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public class CaseModel
    {
        public string Value { get; set; }

        public IList<RuleModelBase> Rules { get; set; }

        public CaseModel()
        {
            Rules = new List<RuleModelBase>();
        }
    }
}