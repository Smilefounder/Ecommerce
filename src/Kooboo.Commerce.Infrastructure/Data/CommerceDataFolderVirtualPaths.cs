using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data
{
    public static class CommerceDataFolderVirtualPaths
    {
        public static string Root
        {
            get
            {
                return "/Commerce_Data";
            }
        }

        public static string Instances
        {
            get
            {
                return UrlUtility.Combine(Root, "Instances");
            }
        }

        public static string ForInstance(string instanceName)
        {
            return UrlUtility.Combine(Instances, instanceName);
        }

        public static string ForInstanceFolder(string instanceName, string folderName)
        {
            return UrlUtility.Combine(ForInstance(instanceName), folderName);
        }
    }
}
