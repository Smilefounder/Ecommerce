using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Kooboo.Commerce.AddIns
{
    public class AddInManager
    {
        private PathInfo _containerPath;

        public AddInManager()
            : this(PathInfo.FromVirtualPath("/AddIns/"))
        {
        }

        public AddInManager(PathInfo containerPath)
        {
            _containerPath = containerPath;
        }

        public ITypeFinder GetTypeFinder(string addInId)
        {
            return new AddInTypeFinder(addInId);
        }

        public void Startup(string addInId)
        {
        }

        public void Shutdown(string addInId)
        {
        }

        public AddInMeta GetAddInMeta(string addInId)
        {
            Require.NotNullOrEmpty(addInId, "addInId");

            var path = new AddInPath(addInId);
            if (Directory.Exists(path.PhysicalPath))
            {
                return new AddInFolder(path).Meta;
            }

            return null;
        }
        
        public IEnumerable<AddInMeta> GetAllAddInMetas()
        {
            var container = new DirectoryInfo(_containerPath.PhysicalPath);

            if (container.Exists)
            {
                foreach (var directory in container.EnumerateDirectories())
                {
                    if (AddInFolder.IsAddInFolder(directory.FullName))
                    {
                        yield return new AddInFolder(PathInfo.Combine(_containerPath, directory.Name)).Meta;
                    }
                }
            }
        }
    }
}
