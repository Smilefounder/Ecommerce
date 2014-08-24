using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine
{
    public class UserItemPair : IEquatable<UserItemPair>
    {
        public string UserId { get; private set; }

        public string ItemId { get; private set; }

        public UserItemPair(string userId, string itemId)
        {
            UserId = userId;
            ItemId = itemId;
        }

        public bool Equals(UserItemPair other)
        {
            return other != null && UserId == other.UserId && ItemId == other.ItemId;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as UserItemPair);
        }

        public override int GetHashCode()
        {
            return UserId.GetHashCode() * 397 ^ ItemId.GetHashCode();
        }
    }
}