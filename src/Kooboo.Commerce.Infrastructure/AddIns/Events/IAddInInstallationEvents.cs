using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns.Events
{
    public interface IAddInInstallationEvents
    {
        void OnInstalled(AddInMeta addIn);
    }

    public interface IAddInUninstallationEvents
    {
        void OnUninstalling(AddInMeta addIn);
    }
}
