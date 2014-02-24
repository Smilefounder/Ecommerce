using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.SqlServer
{
    internal static class MetadataItemExtensions
    {
        public static T GetMetadataPropertyValue<T>(this MetadataItem item, string propertyName)
        {
            Require.NotNull(item, "item");

            var property = item.MetadataProperties.FirstOrDefault(p => p.Name == propertyName);
            return property == null ? default(T) : (T)property.Value;
        }
    }
}
