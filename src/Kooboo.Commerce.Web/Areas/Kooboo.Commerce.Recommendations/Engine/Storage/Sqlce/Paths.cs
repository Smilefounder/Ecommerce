using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce
{
    static class Paths
    {
        public static string Database(string instance, string databaseName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Commerce_Data\\Instances\\" + instance + "\\Recommendations\\" + databaseName + ".sdf");
        }
    }
}