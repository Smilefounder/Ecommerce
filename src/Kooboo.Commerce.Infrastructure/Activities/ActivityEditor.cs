using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Activities
{
    public class ActivityEditor
    {
        public string VirtualPath { get; set; }

        public ActivityEditor(string virtualPath)
        {
            Require.NotNullOrEmpty(virtualPath, "virtualPath");
            VirtualPath = virtualPath;
        }
    }
}
