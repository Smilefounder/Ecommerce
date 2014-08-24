using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Behaviors
{
    public interface IUserItemRelationReader
    {
        /// <summary>
        /// 获取指定用户产生过行为的物品id集合。
        /// </summary>
        ISet<string> GetItemsBehavedBy(string userId);

        /// <summary>
        /// 获取指定用户产生过行为的物品总数。
        /// </summary>
        int GetTotalBehavedItems(string userId);

        /// <summary>
        /// 获取对指定物品产生过行为的所有用户id集合。
        /// </summary>
        ISet<string> GetUsersBehaved(string itemId);
    }
}