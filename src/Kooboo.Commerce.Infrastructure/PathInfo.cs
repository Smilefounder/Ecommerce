using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Kooboo.Commerce
{
    [DebuggerDisplay("{VirtualPath}")]
    public class PathInfo
    {
        public string VirtualPath { get; private set; }

        public string PhysicalPath { get; private set; }

        public PathInfo(PathInfo path)
            : this(path.VirtualPath, path.PhysicalPath)
        {
        }

        public PathInfo(string virtualPath, string physicalPath)
        {
            VirtualPath = virtualPath;
            PhysicalPath = physicalPath;
        }

        public static PathInfo FromVirtualPath(string virtualPath)
        {
            return new PathInfo(virtualPath, HostingEnvironment.MapPath(virtualPath));
        }

        public static PathInfo Combine(PathInfo path, params string[] paths)
        {
            var addedVirtualPath = UrlUtility.Combine(UrlUtility.Combine(paths));
            var addedPhysicalPath = addedVirtualPath.Replace('/', '\\');

            var virtualPath = UrlUtility.Combine(path.VirtualPath, addedVirtualPath);
            var physicalPath = Path.Combine(path.PhysicalPath, addedVirtualPath.TrimStart('\\'));

            return new PathInfo(virtualPath, physicalPath);
        }
    }
}
