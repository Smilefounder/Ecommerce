using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Behaviors
{
    public class BehaviorRecord
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public string ItemId { get; set; }

        public string Type { get; set; }

        public double Weight { get; set; }

        public DateTime UtcTimestamp { get; set; }

        public BehaviorRecord() { }

        public BehaviorRecord(Behavior behavior)
        {
            UserId = behavior.UserId;
            ItemId = behavior.ItemId;
            Type = behavior.Type;
            Weight = behavior.Weight;
            UtcTimestamp = behavior.UtcTimestamp;
        }

        public Behavior ToBehavior()
        {
            return new Behavior
            {
                UserId = UserId,
                ItemId = ItemId,
                Type = Type,
                Weight = Weight,
                UtcTimestamp = UtcTimestamp
            };
        }
    }
}