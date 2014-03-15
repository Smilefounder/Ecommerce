using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.DataSources
{
    [Dependency(typeof(IDataSource), Key = "genders")]
    public class GendersDataSource : IDataSource
    {
        public string Id
        {
            get { return "genders"; }
        }

        public IEnumerable<string> SupportedParameters
        {
            get
            {
                yield return "CustomerName";
            }
        }

        public IEnumerable<IComparisonOperator> SupportedOperators
        {
            get
            {
                return new[] {
                    ComparisonOperators.Equals,
                    ComparisonOperators.NotEquals
                };
            }
        }

        public IEnumerable<ListItem> GetItems(IConditionParameter param)
        {
            return new List<ListItem>
            {
                new ListItem { Text = Gender.Male.ToString(), Value = Gender.Male.ToString() },
                new ListItem { Text = Gender.Female.ToString(), Value = Gender.Female.ToString() },
                new ListItem { Text = Gender.Unknown.ToString(), Value = Gender.Unknown.ToString() }
            };
        }
    }
}
