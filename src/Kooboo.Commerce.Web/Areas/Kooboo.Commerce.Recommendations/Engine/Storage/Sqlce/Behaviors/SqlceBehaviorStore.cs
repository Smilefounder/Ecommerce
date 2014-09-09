using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using Kooboo.Commerce.Recommendations.Engine.Collaborative;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Behaviors
{
    public class SqlceBehaviorStore : IBehaviorStore
    {
        public string InstanceName { get; private set; }

        public string BehaviorType { get; private set; }

        public SqlceBehaviorStore(string instanceName, string behaviorType)
        {
            Require.NotNullOrEmpty(instanceName, "instanceName");
            Require.NotNullOrEmpty(behaviorType, "behaviorType");

            InstanceName = instanceName;
            BehaviorType = behaviorType;
        }

        public IEnumerable<string> GetAllItems()
        {
            using (var db = CreateDbContext())
            {
                return db.Behaviors.Where(it => it.Type == BehaviorType)
                                   .GroupBy(it => it.ItemId)
                                   .Select(it => it.Key)
                                   .ToList();
            }
        }

        public DateTime GetBehaviorTimestamp(string userId, string itemId)
        {
            using (var db = CreateDbContext())
            {
                var timestamp = db.Behaviors
                                  .Where(it => it.Type == BehaviorType && it.UserId == userId && it.ItemId == itemId)
                                  .Select(it => it.UtcTimestamp)
                                  .FirstOrDefault();

                return timestamp;
            }
        }

        public void SaveBehaviors(IEnumerable<Behavior> behaviors)
        {
            using (var db = CreateDbContext())
            {
                foreach (var behavior in behaviors)
                {
                    var record = new BehaviorRecord(behavior);

                    try
                    {
                        db.Behaviors.Add(record);
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        db.Entry(record).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
        }

        public IEnumerable<Behavior> GetRecentBehaviors(int count)
        {
            using (var db = CreateDbContext())
            {
                var behaviors = db.Behaviors.Where(it => it.Type == BehaviorType)
                                            .OrderByDescending(it => it.UtcTimestamp)
                                            .Take(count)
                                            .ToList();
                return behaviors.Select(it => it.ToBehavior()).ToList();
            }
        }

        public IEnumerable<string> GetItemsUserHadBehaviorsOn(string userId, int take)
        {
            using (var db = CreateDbContext())
            {
                var query = db.Behaviors.Where(it => it.Type == BehaviorType && it.UserId == userId)
                                   .OrderByDescending(it => it.UtcTimestamp)
                                   .Select(it => it.ItemId)
                                   .Distinct();

                if (take > 0)
                {
                    query = query.Take(take);
                }

                return query.ToList();
            }
        }

        public int GetUserActiveRate(string userId)
        {
            using (var db = CreateDbContext())
            {
                return db.Behaviors.Where(it => it.Type == BehaviorType && it.UserId == userId)
                                   .Select(it => it.ItemId)
                                   .Distinct()
                                   .Count();
            }
        }

        public int GetTotalUsersHadBehaviorsOn(string itemId)
        {
            using (var db = CreateDbContext())
            {
                return db.Behaviors.Where(it => it.Type == BehaviorType && it.ItemId == itemId).Count();
            }
        }

        public IEnumerable<string> GetUsersHadBehaviorsOnBoth(string item1, string item2)
        {
            using (var db = CreateDbContext())
            {
                return db.Database.SqlQuery<string>(
                        "select distinct UserId from BehaviorRecords t1 where ItemId = @p0 and exists (select * from BehaviorRecords t2 where t2.UserId = t1.UserId and t2.ItemId = @p1)",
                        new object[] { item1, item2 }).ToList();
            }
        }

        private BehaviorDbContext CreateDbContext()
        {
            return new BehaviorDbContext(InstanceName, BehaviorType);
        }
    }
}