using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns
{
    // TODO: Compare for file version
    public class AssemblyInfo
    {
        public string AssemblyName { get; private set; }

        public string Version { get; private set; }

        public AssemblyInfo(string assemblyName, string version)
        {
            AssemblyName = assemblyName;
            Version = version;
        }
    }
}
