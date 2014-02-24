using Kooboo.CMS.Common.Runtime;
using Kooboo.Commerce.AddIns.Events;
using Kooboo.Commerce.AddIns.Migration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns
{
    public class AddInMigrator : IAddInMigrator
    {
        private IAssemblyReferencingService _assemblyReferencingService;

        public AddInMigrator(IAssemblyReferencingService assemblyReferencingService)
        {
            _assemblyReferencingService = assemblyReferencingService;
        }

        public AddInPreparationResult Prepare(PathInfo addInTempInstallationPath)
        {
            var result = new AddInPreparationResult
            {
                IsValid = true
            };

            var tempAddInFolder = new AddInFolder(addInTempInstallationPath);
            var validationResults = tempAddInFolder.Validate();

            if (validationResults.Count > 0)
            {
                result.IsValid = false;
                result.Errors = validationResults.Select(x => x.ErrorMessage).ToList();
            }

            if (result.IsValid)
            {
                var newAssemblies = tempAddInFolder.GetAssemblies();
                var currentAssemblies = _assemblyReferencingService.GetReferencedAssemblies(tempAddInFolder.Meta.Id);
                var currentSharedAssemblies = new List<AssemblyInfo>();

                foreach (var assembly in currentAssemblies)
                {
                    var referenings = _assemblyReferencingService.GetReferencingAddIns(assembly);
                    if (referenings.Any(x => x != tempAddInFolder.Meta.Id))
                    {
                        currentSharedAssemblies.Add(assembly);
                    }
                }

                result.AssemblyConflicts = _assemblyReferencingService.GetAssemlbyConflicts(currentSharedAssemblies).ToList();

                MarkCurrentVersion(addInTempInstallationPath.PhysicalPath, new AddInFolder(new AddInPath(tempAddInFolder.Meta.Id)).Meta.Version);
            }

            return result;
        }

        private void MarkCurrentVersion(string addInTempInstallationPath, string currentVersion)
        {
            var markerPath = Path.Combine(addInTempInstallationPath, "old-version.txt");
            File.WriteAllText(markerPath, currentVersion, Encoding.UTF8);
        }

        private string GetCurrentVersion(string addInTempInstallationPath)
        {
            var markerPath = Path.Combine(addInTempInstallationPath, "old-version.txt");
            return File.ReadLines(markerPath, Encoding.UTF8).FirstOrDefault();
        }

        public void DeployFiles(string addInId, AddInInstallationOptions options)
        {
            var tempAddInFolder = new AddInFolder(new AddInTempInstallationPath(addInId));

            if (!Directory.Exists(tempAddInFolder.Path.PhysicalPath))
                throw new DirectoryNotFoundException("Add-in temp installation directory was deleted unexpected.");

            var binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin");

            var currentAssemblies = _assemblyReferencingService.GetReferencedAssemblies(addInId);

            // Delete old assemblies only used by this addin
            foreach (var currentAssembly in currentAssemblies)
            {
                var targetPath = Path.Combine(binPath, currentAssembly.AssemblyName + ".dll");
                var referencings = _assemblyReferencingService.GetReferencingAddIns(currentAssembly).ToList();

                if (referencings.Count == 1 && referencings[0] == addInId)
                {
                    File.Delete(targetPath);
                }
            }

            // Deploy assemblies
            foreach (var newAssembly in tempAddInFolder.GetAssemblies())
            {
                var conflictSolution = options.AssemblyConflictSolutions.FirstOrDefault(x => x.AssemblyName == newAssembly.AssemblyName);

                // If conflict solution is null, then this assembly is not conflicted assembly
                if (conflictSolution != null && !conflictSolution.Override)
                {
                    continue;
                }

                var sourcePath = Path.Combine(tempAddInFolder.BinPath.PhysicalPath, newAssembly.AssemblyName + ".dll");
                var targetPath = Path.Combine(binPath, newAssembly.AssemblyName + ".dll");

                File.Copy(sourcePath, targetPath, true);

                _assemblyReferencingService.AddReference(newAssembly, addInId);
            }

            // Deploy views, assets, etc
            var addInPath = new AddInPath(addInId);
            var currentAddInFolder = new AddInFolder(addInPath);
            var currentVersion = currentAddInFolder.Meta.Version;

            Kooboo.IO.IOUtility.DeleteDirectory(addInPath.PhysicalPath, true);
            tempAddInFolder.CopyTo(addInPath);
        }

        public void RunMigration(string addInId, AddInInstallationOptions options)
        {
            var typeFinder = new AddInTypeFinder(addInId, _assemblyReferencingService);
            var events = CreateEvents(typeFinder);

            var tempAddInFolder = new AddInFolder(new AddInTempInstallationPath(addInId));
            var sourceVersion = Version.Parse(GetCurrentVersion(tempAddInFolder.Path.PhysicalPath));
            var targetVersion = Version.Parse(tempAddInFolder.Meta.Version);
            var isUpgrade = targetVersion > sourceVersion;

            // Pre events
            if (events != null && !isUpgrade)
            {
                events.OnDowngrading(tempAddInFolder.Meta);
            }

            var upgradeActions = UpgradeActionFinder.GetUpgradeActions(
                addInId, sourceVersion, targetVersion, _assemblyReferencingService);

            // Run upgrade actions
            foreach (var action in upgradeActions)
            {
                if (isUpgrade)
                {
                    action.Do();
                }
                else
                {
                    action.Undo();
                }
            }

            // Post events
            if (events != null && isUpgrade)
            {
                events.OnUpgraded(tempAddInFolder.Meta);
            }
        }

        private IAddInMigrationEvents CreateEvents(ITypeFinder typeFinder)
        {
            var eventType = typeFinder.FindClassesOfType<IAddInMigrationEvents>().FirstOrDefault();
            if (eventType != null)
            {
                return (IAddInMigrationEvents)Activator.CreateInstance(eventType);
            }

            return null;
        }
    }
}
