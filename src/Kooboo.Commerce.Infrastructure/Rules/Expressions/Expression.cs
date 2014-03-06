using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Expressions
{
    /// <summary>
    /// Represents a node in the abstract syntax tree.
    /// </summary>
    public abstract class Expression
    {
        public abstract void Accept(IExpressionVisitor visitor);
    }
}
