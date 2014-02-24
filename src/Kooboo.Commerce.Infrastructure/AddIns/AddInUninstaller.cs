using Kooboo.Commerce.AddIns.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns
{
    public class AddInUninstaller : IAddInUninstaller
    {
        private IAssemblyReferencingService _assemblyReferencingService;

        public AddInUninstaller(IAssemblyReferencingService assemblyReferencingService)
        {
            _assemblyReferencingService = assemblyReferencingService;
        }

        public void RunUninstallation(string addInId)
        {
            var addInFolder = new AddInFolder(new AddInPath(addInId));

            RunEvents(addInFolder.Meta);
            DeleteFiles(addInId, addInFolder);
        }

        private void DeleteFiles(string addInId, AddInFolder addInFolder)
        {
            var binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin");
            var referencedAssemblies = _assemblyReferencingService.GetReferencedAssemblies(addInId);

            foreach (var assembly in referencedAssemblies)
            {
                _assemblyReferencingService.RemoveReference(assembly, addInId);

                var referencings = _assemblyReferencingService.GetReferencingAddIns(assembly);
                if (!referencings.Any())
                {
                    // Not referenced by others, delete it
                    var dllPath = Path.Combine(binPath, assembly.AssemblyName + ".dll");
                    File.Delete(dllPath);
                }
            }

            // Delete views, assets
            Kooboo.IO.IOUtility.DeleteDirectory(addInFolder.Path.PhysicalPath, true);
        }

        private void RunEvents(AddInMeta addIn)
        {
            var typeFinder = new AddInTypeFinder(addIn.Id, _assemblyReferencingService);
            var eventType = typeFinder.FindClassesOfType<IAddInUninstallationEvents>().FirstOrDefault();
            if (eventType != null)
            {
                var events = (IAddInUninstallationEvents)Activator.CreateInstance(eventType);
                events.OnUninstalling(addIn);
            }
        }
    }
}
