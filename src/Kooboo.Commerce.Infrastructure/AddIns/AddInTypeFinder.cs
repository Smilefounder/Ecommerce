using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Compilation;

namespace Kooboo.Commerce.AddIns
{
    public class AddInTypeFinder : WebAppTypeFinder
    {
        private IAssemblyReferencingService _assemblyReferencingService;

        public string AddInId { get; private set; }

        public AddInTypeFinder(string addInId)
            : this(addInId, AssemblyReferencingServices.Current)
        {
        }

        public AddInTypeFinder(string addInId, IAssemblyReferencingService assemblyReferencingService)
        {
            Require.NotNullOrEmpty(addInId, "addInId");
            Require.NotNull(assemblyReferencingService, "assemblyReferencingService");

            AddInId = addInId;
            _assemblyReferencingService = assemblyReferencingService;
        }

        public override IList<System.Reflection.Assembly> GetAssemblies()
        {
            var addInAssemblies = _assemblyReferencingService.GetReferencedAssemblies(AddInId);
            var systemAssemblies = _assemblyReferencingService.GetReferencedAssemblies("System");

            var result = new List<Assembly>();
            var binDirectory = GetBinDirectory();

            foreach (var assemblyInfo in addInAssemblies.Except(systemAssemblies))
            {
                var asmPath = Path.Combine(binDirectory, assemblyInfo.AssemblyName + ".dll");
                var asmName = AssemblyName.GetAssemblyName(asmPath);
                var assembly = App.Load(asmName);
                result.Add(assembly);
            }

            return result;
        }
    }
}
