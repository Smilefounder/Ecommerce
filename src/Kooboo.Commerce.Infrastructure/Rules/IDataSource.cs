using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IDataSource
    {
        string Id { get; }

        IEnumerable<string> SupportedParameters { get; }

        IEnumerable<IComparisonOperator> SupportedOperators { get; }

        IEnumerable<ListItem> GetItems(IConditionParameter param);
    }

    public class ListItem
    {
        public string Text { get; set; }

        public string Value { get; set; }
    }
}
