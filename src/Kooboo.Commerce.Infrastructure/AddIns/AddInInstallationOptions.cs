using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns
{
    public class AddInInstallationOptions
    {
        public IList<AssemblyConflictSolution> AssemblyConflictSolutions { get; set; }

        public AddInInstallationOptions()
        {
            AssemblyConflictSolutions = new List<AssemblyConflictSolution>();
        }
    }
}
