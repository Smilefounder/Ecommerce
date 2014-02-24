using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns
{
    public interface IAddInMigrator
    {
        AddInPreparationResult Prepare(PathInfo addInTempInstallationPath);

        void DeployFiles(string addInId, AddInInstallationOptions options);

        void RunMigration(string addInId, AddInInstallationOptions options);
    }
}
