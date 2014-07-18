using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Web.Framework.UI.Tabs.Queries
{
    public class SavedTabQuery
    {
        public Guid Id { get; set; }

        public string QueryName { get; set; }

        public string DisplayName { get; set; }

        public int Order { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public object Config { get; set; }

        public SavedTabQuery()
        {
            Id = Guid.NewGuid();
            CreatedAtUtc = DateTime.UtcNow;
        }

        public static SavedTabQuery CreateFrom(ITabQuery query)
        {
            return new SavedTabQuery
            {
                QueryName = query.Name,
                DisplayName = query.DisplayName,
                Config = query.ConfigType == null ? null : Activator.CreateInstance(query.ConfigType)
            };
        }
    }
}
