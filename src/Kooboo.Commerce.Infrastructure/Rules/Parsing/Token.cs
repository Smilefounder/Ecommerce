using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parsing
{
    public class Token
    {
        public TokenKind Kind { get; private set; }

        public SourceLocation Location { get; private set; }

        public string Value { get; private set; }

        public Token(string value, TokenKind kind, SourceLocation location)
        {
            Value = value;
            Kind = kind;
            Location = location;
        }

        public override string ToString()
        {
            return Kind + ": " + Value;
        }
    }
}
