using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns
{
    public class AssemblyConflict
    {
        public string AssemblyName { get; set; }

        public string CurrentVersion { get; set; }

        public string NewVersion { get; set; }

        public AssemblyConflict(string assemblyName, string currentVersion, string newVersion)
        {
            AssemblyName = assemblyName;
            CurrentVersion = currentVersion;
            NewVersion = newVersion;
        }
    }
}
