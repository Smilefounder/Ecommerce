using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Parsing
{
    public enum TokenKind
    {
        Identifier,
        StringLiteral,
        Number,
        And,
        Or,
        Parenthesis,
        DoubleColon,
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }
}
