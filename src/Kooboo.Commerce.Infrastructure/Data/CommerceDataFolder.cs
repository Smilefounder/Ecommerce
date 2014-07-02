using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    static class CommerceDataFolder
    {
        public static string RootPath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Commerce_Data");
            }
        }

        public static string InstancesRootPath
        {
            get
            {
                return Path.Combine(RootPath, "Instances");
            }
        }

        public static string GetInstancePath(string instanceName)
        {
            return Path.Combine(InstancesRootPath, instanceName);
        }

        public static string GetInstanceFolderPath(string instanceName, string folderName)
        {
            return Path.Combine(GetInstancePath(instanceName), folderName);
        }
    }
}
