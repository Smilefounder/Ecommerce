using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns.Migration
{
    static class UpgradeActionFinder
    {
        public static IEnumerable<IUpgradeAction> GetUpgradeActions(string addInId, Version currentVersion, Version targetVersion, IAssemblyReferencingService assemblyReferencingService)
        {
            if (currentVersion == targetVersion)
            {
                return Enumerable.Empty<IUpgradeAction>();
            }

            var typeFinder = new AddInTypeFinder(addInId, assemblyReferencingService);
            IEnumerable<UpgradeActionDescriptor> descriptors = typeFinder.FindClassesOfType<IUpgradeAction>()
                                                                         .Select(x => new UpgradeActionDescriptor(x))
                                                                         .ToList();

            var isUpgrade = currentVersion < targetVersion;
            var higherVersion = isUpgrade ? targetVersion : currentVersion;
            var lowerVersion = isUpgrade ? currentVersion : targetVersion;

            descriptors = from descriptor in descriptors
                          let version = Version.Parse(descriptor.Attribute.Version)
                          where version > lowerVersion && version <= higherVersion
                          select descriptor;

            if (isUpgrade)
            {
                descriptors = descriptors.OrderBy(x => x.Attribute.Version)
                                         .ThenBy(x => x.Attribute.Order);
            }
            else
            {
                descriptors = descriptors.OrderByDescending(x => x.Attribute.Version)
                                         .ThenByDescending(x => x.Attribute.Order);
            }

            return descriptors.Select(x => (IUpgradeAction)Activator.CreateInstance(x.ActionType)).ToList();
        }
    }
}
