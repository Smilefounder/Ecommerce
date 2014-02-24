using Kooboo.Commerce.AddIns.Migration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns
{
    public interface IAddInInstaller
    {
        AddInPreparationResult Prepare(PathInfo tempInstallationPath);

        void DeployFiles(string addInId, AddInInstallationOptions options);

        void RunInstallation(string addInId, AddInInstallationOptions options);
    }
}
