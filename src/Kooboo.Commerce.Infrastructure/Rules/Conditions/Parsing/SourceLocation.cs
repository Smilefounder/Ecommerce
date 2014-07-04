using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Conditions.Parsing
{
    /// <summary>
    /// Represents a location in the rule condition expression. 
    /// Currently only support single line expressions, so only CharIndex is needed.
    /// </summary>
    public struct SourceLocation : IEquatable<SourceLocation>
    {
        private int _charIndex;

        public int CharIndex
        {
            get
            {
                return _charIndex;
            }
        }

        public SourceLocation(int charIndex)
        {
            Require.That(charIndex >= 0, "charIndex", "Char index cannot be less than zero.");
            _charIndex = charIndex;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is SourceLocation)
            {
                return Equals((SourceLocation)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return CharIndex;
        }

        public bool Equals(SourceLocation other)
        {
            return CharIndex == other.CharIndex;
        }

        public override string ToString()
        {
            return "Char " + (CharIndex + 1);
        }
    }
}
