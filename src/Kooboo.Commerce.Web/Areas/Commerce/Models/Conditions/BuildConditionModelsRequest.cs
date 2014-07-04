using Kooboo.Commerce.Rules;
using Kooboo.Commerce.Rules.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Conditions
{
    public class BuildConditionModelsRequest
    {
        public string DataContextType { get; set; }

        public List<Condition> Conditions { get; set; }

        public BuildConditionModelsRequest()
        {
            Conditions = new List<Condition>();
        }
    }
}