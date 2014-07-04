using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Conditions.Parsing
{
    public class ParsingContext
    {
        public IList<Error> Errors { get; private set; }

        public ISet<string> RegisteredComparisonOperators { get; private set; }

        public ParsingContext(IEnumerable<string> registeredComparisonOperators)
        {
            Errors = new List<Error>();
            RegisteredComparisonOperators = new HashSet<string>(registeredComparisonOperators);
        }

        public void AddError(string message, SourceLocation location)
        {
            Require.NotNullOrEmpty(message, "message");
            Errors.Add(new Error(message, location));
        }
    }
}
