using Kooboo.Commerce.Recommendations.Engine.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Behaviors
{
    public class BehaviorRecord
    {
        [Key, Column(Order = 0), StringLength(50)]
        public string Type { get; set; }

        [Key, Column(Order = 1), StringLength(50)]
        public string UserId { get; set; }

        [Key, Column(Order = 2), StringLength(50)]
        public string ItemId { get; set; }

        public double Weight { get; set; }

        public DateTime UtcTimestamp { get; set; }

        public BehaviorRecord() { }

        public BehaviorRecord(Behavior behavior)
        {
            Type = behavior.Type;
            UserId = behavior.UserId;
            ItemId = behavior.ItemId;
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