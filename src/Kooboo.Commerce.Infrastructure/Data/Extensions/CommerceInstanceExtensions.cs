using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Folders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public static class CommerceInstanceExtensions
    {
        public static DataFolder GetDataFolder(this CommerceInstance instance)
        {
            return DataFolders.Instances.GetFolder(instance.Name);
        }
    }
}
