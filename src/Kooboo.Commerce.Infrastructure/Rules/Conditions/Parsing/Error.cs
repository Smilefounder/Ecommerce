using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parsing
{
    public class Error
    {
        public string Message { get; private set; }

        public SourceLocation Location { get; private set; }

        public Error(string message, SourceLocation location)
        {
            Message = message;
            Location = location;
        }

        public override string ToString()
        {
            return "Char " + (Location.CharIndex + 1) + ": " + Message;
        }
    }
}
