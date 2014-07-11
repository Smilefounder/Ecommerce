using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Folders.Disk
{
    public class DiskDataFolderFactory : DataFolderFactory
    {
        public override DataFolder GetFolder(string virtualPath, IDataFileFormat defaultFileFormat)
        {
            return new DiskDataFolder(virtualPath, defaultFileFormat);
        }

        public override DataFile GetFile(string virtualPath, IDataFileFormat format)
        {
            return new DiskDataFile(virtualPath, format);
        }
    }
}
