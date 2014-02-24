using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns
{
    public class AssemblyConflictSolution
    {
        public string AssemblyName { get; set; }

        public bool Override { get; set; }

        public AssemblyConflictSolution(string assemblyName, bool @override)
        {
            AssemblyName = assemblyName;
            Override = @override;
        }
    }
}
