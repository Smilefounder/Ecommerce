using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Folders
{
    public class CommerceInstanceDataFolders
    {
        public DataFolder Root { get; private set; }

        public DataFolder Media { get; private set; }

        public CommerceInstanceDataFolders(CommerceInstance instance)
        {
            Root = DataFolders.Instances.GetFolder(instance.Name);
            Media = Root.GetFolder("Media");
        }
    }
}