using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Kooboo.Commerce.AddIns
{
    public class AddInPath : PathInfo
    {
        public AddInPath(string addInId)
            : base(PathInfo.FromVirtualPath("/Areas/" + addInId))
        {
            Require.NotNullOrEmpty(addInId, "addInId");
        }
    }

    public class AddInTempInstallationPath : PathInfo
    {
        public AddInTempInstallationPath(string addInId)
            : base(PathInfo.FromVirtualPath("/App_Data/AddInInstallationTemp/" + addInId))
        {
            Require.NotNullOrEmpty(addInId, "addInId");
        }
    }
}
