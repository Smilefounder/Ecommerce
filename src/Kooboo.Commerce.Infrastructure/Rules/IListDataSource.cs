using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules
{
    public interface IListDataSource
    {
        IEnumerable<string> SupportedParameters { get; }

        IEnumerable<IComparisonOperator> SupportedOperators { get; }

        IEnumerable<ListItem> GetItems(IParameter param);
    }

    public class ListItem
    {
        public string Text { get; set; }

        public string Value { get; set; }
    }
}
