using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IParameterValueResolver
    {
        object GetValue(ConditionParameter param, object dataContext);
    }

    class DumbParameterValueResolver : IParameterValueResolver
    {
        public static readonly DumbParameterValueResolver Instance = new DumbParameterValueResolver();

        public object GetValue(ConditionParameter param, object dataContext)
        {
            return dataContext;
        }
    }
}
