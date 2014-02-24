using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce
{
    public static class AssembliesExtensions
    {
        public static Assembly FindWithoutVersion(this IEnumerable<Assembly> assemblies, string assemblyName)
        {
            var shortName = GetAssemblyShortName(assemblyName);
            return assemblies.FirstOrDefault(x => GetAssemblyShortName(x.FullName) == shortName);
        }

        static string GetAssemblyShortName(string assemblyName)
        {
            var index = assemblyName.IndexOf(',');
            if (index < 0)
            {
                return assemblyName;
            }

            return assemblyName.Substring(0, index);
        }
    }
}
