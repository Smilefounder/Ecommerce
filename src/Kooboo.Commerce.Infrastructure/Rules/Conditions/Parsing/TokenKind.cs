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
        AND,
        OR,
        Parenthesis,
        ComparisonOperator
    }
}
