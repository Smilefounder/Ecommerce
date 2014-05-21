using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Promotions
{
    public class PromotionPolicyEditor
    {
        public string VirtualPath { get; private set; }

        public PromotionPolicyEditor(string virtualPath)
        {
            Require.NotNullOrEmpty(virtualPath, "virtualPath");
            VirtualPath = virtualPath;
        }
    }
}
