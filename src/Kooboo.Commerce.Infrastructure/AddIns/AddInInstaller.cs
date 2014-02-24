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
    public class AddInInstaller : IAddInInstaller
    {
        private IAssemblyReferencingService _assemblyReferencingService;

        public AddInInstaller(IAssemblyReferencingService assemblyReferencingService)
        {
            _assemblyReferencingService = assemblyReferencingService;
        }

        public AddInPreparationResult Prepare(PathInfo tempInstallationPath)
        {
            var result = new AddInPreparationResult
            {
                IsValid = true
            };

            var addInFolder = new AddInFolder(tempInstallationPath);
            var validationResults = addInFolder.Validate();

            if (validationResults.Count > 0)
            {
                result.IsValid = false;
                result.Errors = validationResults.Select(x => x.ErrorMessage).ToList();
            }

            result.AssemblyConflicts = _assemblyReferencingService.GetAssemlbyConflicts(addInFolder.GetAssemblies()).ToList();

            return result;
        }

        public void DeployFiles(string addInId, AddInInstallationOptions options)
        {
            var tempInstallationPath = new AddInTempInstallationPath(addInId);

            if (!Directory.Exists(tempInstallationPath.PhysicalPath))
                throw new DirectoryNotFoundException("Add-In temp installation folder was deleted unexpected.");

            var tempAddInFolder = new AddInFolder(tempInstallationPath);

            // Deploy assemblies
            foreach (var assemblyInfo in tempAddInFolder.GetAssemblies())
            {
                var confictSolution = options.AssemblyConflictSolutions.FirstOrDefault(x => x.AssemblyName == assemblyInfo.AssemblyName);

                // If conflict solution is null, then this assembly is not conflicted assembly
                if (confictSolution != null && !confictSolution.Override)
                {
                    continue;
                }

                var sourcePath = Path.Combine(tempAddInFolder.BinPath.PhysicalPath, assemblyInfo.AssemblyName + ".dll");
                var targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin\\" + assemblyInfo.AssemblyName + ".dll");

                File.Copy(sourcePath, targetPath, true);

                _assemblyReferencingService.AddReference(assemblyInfo, addInId);
            }

            // Deploy views, assets, etc
            tempAddInFolder.CopyTo(new AddInPath(addInId));
        }

        public void RunInstallation(string addInId, AddInInstallationOptions options)
        {
            var tempPath = new AddInTempInstallationPath(addInId);

            if (Directory.Exists(tempPath.PhysicalPath))
            {
                Kooboo.IO.IOUtility.DeleteDirectory(tempPath.PhysicalPath, true);
            }

            var addInFolder = new AddInFolder(new AddInPath(addInId));
            RunEvents(addInFolder.Meta);
        }

        private void RunEvents(AddInMeta addIn)
        {
            var typeFinder = new AddInTypeFinder(addIn.Id, _assemblyReferencingService);
            var eventType = typeFinder.FindClassesOfType<IAddInInstallationEvents>().FirstOrDefault();
            if (eventType != null)
            {
                var events = (IAddInInstallationEvents)Activator.CreateInstance(eventType);
                events.OnInstalled(addIn);
            }
        }
    }
}
