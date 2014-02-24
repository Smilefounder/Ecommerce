using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public class ActivityBinding
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [StringLength(300)]
        public string EventClrType { get; set; }

        [Required]
        [StringLength(300)]
        public string ActivityName { get; set; }

        public string ActivityData { get; set; }

        public bool IsEnabled { get; protected set; }

        public int Priority { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public ActivityBinding()
        {
            CreatedAtUtc = DateTime.UtcNow;
        }

        public virtual void Enable()
        {
            if (!IsEnabled)
            {
                IsEnabled = true;
            }
        }

        public virtual void Disable()
        {
            if (IsEnabled)
            {
                IsEnabled = false;
            }
        }

        public static ActivityBinding Create(Type eventType, string activityName, string description)
        {
            return new ActivityBinding
            {
                Description = description,
                EventClrType = eventType.GetVersionUnawareAssemblyQualifiedName(),
                ActivityName = activityName
            };
        }
    }
}
