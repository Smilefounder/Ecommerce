using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Behaviors
{
    public class SqlceBehaviorStore : IBehaviorTimestampReader, IUserItemRelationReader
    {
        public string InstanceName { get; private set; }

        public string BehaviorType { get; private set; }

        public SqlceBehaviorStore(string instanceName, string behaviorType)
        {
            InstanceName = instanceName;
            BehaviorType = behaviorType;
        }

        public DateTime GetBehaviorTimestamp(string userId, string itemId)
        {
            using (var db = CreateDbContext())
            {
                var behavior = db.Behaviors
                                 .Where(it => it.Type == BehaviorType)
                                 .Where(it => it.UserId == userId && it.ItemId == itemId)
                                 .OrderByDescending(it => it.UtcTimestamp)
                                 .FirstOrDefault();

                if (behavior == null)
                {
                    return DateTime.MinValue;
                }

                return behavior.UtcTimestamp;
            }
        }

        public void AddBehaviors(IEnumerable<Behavior> behaviors)
        {
            using (var db = CreateDbContext())
            {
                foreach (var behavior in behaviors)
                {
                    db.Behaviors.Add(new BehaviorRecord(behavior));
                }

                db.SaveChanges();
            }
        }

        public IEnumerable<Behavior> GetRecentBehaviors(int count)
        {
            using (var db = CreateDbContext())
            {
                var behaviors = db.Behaviors.OrderByDescending(it => it.UtcTimestamp).Take(count).ToList();
                return behaviors.Select(it => it.ToBehavior()).ToList();
            }
        }

        public ISet<string> GetItemsBehavedBy(string userId)
        {
            using (var db = CreateDbContext())
            {
                var userItems = db.UserItems.Find(userId);
                if (userItems != null && !String.IsNullOrEmpty(userItems.ItemIds))
                {
                    return SetSerializer.Deserialize(userItems.ItemIds);
                }

                return new HashSet<string>();
            }
        }

        public int GetTotalBehavedItems(string userId)
        {
            return GetItemsBehavedBy(userId).Count;
        }

        public ISet<string> GetUsersBehaved(string itemId)
        {
            using (var db = CreateDbContext())
            {
                var itemUsers = db.ItemUsers.Find(itemId);
                if (itemUsers != null && !String.IsNullOrEmpty(itemUsers.UserIds))
                {
                    return SetSerializer.Deserialize(itemUsers.UserIds);
                }

                return new HashSet<string>();
            }
        }

        public void AddUserItems(string userId, IEnumerable<string> items)
        {
            using (var db = CreateDbContext())
            {
                var userItems = db.UserItems.Find(userId);
                if (userItems == null)
                {
                    userItems = new UserItems { UserId = userId };
                    db.UserItems.Add(userItems);
                }

                var itemIds = SetSerializer.Deserialize(userItems.ItemIds);
                itemIds.UnionWith(items);

                userItems.ItemIds = SetSerializer.Serialize(itemIds);

                db.SaveChanges();
            }
        }

        public void AddItemUsers(string itemId, IEnumerable<string> users)
        {
            using (var db = CreateDbContext())
            {
                var itemUsers = db.ItemUsers.Find(itemId);
                if (itemUsers == null)
                {
                    itemUsers = new ItemUsers { ItemId = itemId };
                    db.ItemUsers.Add(itemUsers);
                }

                var userIds = SetSerializer.Deserialize(itemUsers.UserIds);
                userIds.UnionWith(users);

                itemUsers.UserIds = SetSerializer.Serialize(userIds);

                db.SaveChanges();
            }
        }

        private BehaviorDbContext CreateDbContext()
        {
            return new BehaviorDbContext(InstanceName);
        }
    }
}