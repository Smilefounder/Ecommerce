using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Operators
{
    public class GreaterThanOperator : IComparisonOperator
    {
        public string Name
        {
            get { return "greather_than"; }
        }

        public string DisplayName
        {
            get
            {
                return "Greater Than";
            }
        }

        public bool Apply(IParameter param, object paramValue, object inputValue)
        {
            throw new NotImplementedException();
        }
    }
}
