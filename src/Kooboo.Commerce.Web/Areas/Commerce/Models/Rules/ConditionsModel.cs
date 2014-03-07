using Kooboo.Commerce.Rules.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Rules
{
    public class ConditionsModel
    {
        public IList<ConditionModel> Conditions { get; set; }

        public ConditionsModel()
        {
            Conditions = new List<ConditionModel>();
        }
    }
}