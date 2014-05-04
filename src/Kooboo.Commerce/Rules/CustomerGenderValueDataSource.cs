using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    [Dependency(typeof(IParameterValueDataSource), Key = "genders")]
    public class CustomerGenderValueDataSource : IParameterValueDataSource
    {
        public string Name
        {
            get { return "genders"; }
        }

        public string TargetParameterName
        {
            get
            {
                return "CustomerGender";
            }
        }

        public IEnumerable<ParameterValue> GetValues(IConditionParameter param)
        {
            return new List<ParameterValue>
            {
                new ParameterValue { Text = Gender.Male.ToString(), Value = Gender.Male.ToString() },
                new ParameterValue { Text = Gender.Female.ToString(), Value = Gender.Female.ToString() },
                new ParameterValue { Text = Gender.Unknown.ToString(), Value = Gender.Unknown.ToString() }
            };
        }
    }
}
