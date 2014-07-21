using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Tabs.Queries
{
    public class SavedTabQuery
    {
        public Guid Id { get; set; }

        public string QueryName { get; set; }

        [Display(Name = "Display name")]
        [Required]
        public string DisplayName { get; set; }

        public int Order { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public object Config { get; set; }

        private SavedTabQuery()
        {
        }

        public SavedTabQuery(string queryName)
        {
            QueryName = queryName;
            Id = Guid.NewGuid();
            CreatedAtUtc = DateTime.UtcNow;
        }

        public static SavedTabQuery CreateFrom(ITabQuery query, string displayName = null)
        {
            return new SavedTabQuery(query.Name)
            {
                DisplayName = displayName,
                Config = query.ConfigType == null ? null : Activator.CreateInstance(query.ConfigType)
            };
        }
    }
}
