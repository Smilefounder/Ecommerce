using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public class ItemPair : IEquatable<ItemPair>
    {
        public string Item1 { get; private set; }

        public string Item2 { get; private set; }

        public ItemPair(string item1, string item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public bool Equals(ItemPair other)
        {
            return other != null && Item1 == other.Item1 && Item2 == other.Item2;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ItemPair);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Item1.GetHashCode() * 397 ^ Item2.GetHashCode();
            }
        }
    }
}