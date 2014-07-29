using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Settings;

namespace Kooboo.Commerce.Data.Mapping
{
    class KeyValueSettingMap : EntityTypeConfiguration<SettingItem>
    {
        public KeyValueSettingMap()
        {
        }
    }
}
