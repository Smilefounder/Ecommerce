using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parsing
{
    public class ParsingContext
    {
        public IList<Error> Errors { get; private set; }

        public ParsingContext()
        {
            Errors = new List<Error>();
        }

        public void AddError(string message, SourceLocation location)
        {
            Require.NotNullOrEmpty(message, "message");
            Errors.Add(new Error(message, location));
        }
    }
}
