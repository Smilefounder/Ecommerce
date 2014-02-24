using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns
{
    public interface IAssemblyReferencingService
    {
        IEnumerable<AssemblyConflict> GetAssemlbyConflicts(IEnumerable<AssemblyInfo> assemblyInfos);

        IEnumerable<AssemblyInfo> GetReferencedAssemblies(string addInId);

        IEnumerable<string> GetReferencingAddIns(AssemblyInfo assemblyInfo);

        void AddReference(AssemblyInfo assemblyInfo, string addInId);

        bool RemoveReference(AssemblyInfo assemblyInfo, string addInId);
    }

    public static class AssemblyReferencingServices
    {
        public static IAssemblyReferencingService Current = new AssemblyReferencingService();
    }
}
