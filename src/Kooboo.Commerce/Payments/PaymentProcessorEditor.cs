using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Payments
{
    public class PaymentProcessorEditor
    {
        public string VirtualPath { get; private set; }

        public PaymentProcessorEditor(string virtualPath)
        {
            Require.NotNullOrEmpty(virtualPath, "virtualPath");
            VirtualPath = virtualPath;
        }
    }
}
