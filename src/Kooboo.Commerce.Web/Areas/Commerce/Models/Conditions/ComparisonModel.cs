using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Conditions
{
    public class ComparisonModel
    {
        public string ParamName { get; set; }

        public string Operator { get; set; }

        public string OperatorDisplayName { get; set; }

        public string Value { get; set; }

        public string ValueType { get; set; }

        public bool IsNumberValue { get; set; }

        public string GetExpression()
        {
            var value = Value;
            if (!IsNumberValue)
            {
                value = "\"" + value + "\"";
            }

            return ParamName + " " + OperatorDisplayName + " " + value;
        }

        public override string ToString()
        {
            return GetExpression();
        }
    }
}