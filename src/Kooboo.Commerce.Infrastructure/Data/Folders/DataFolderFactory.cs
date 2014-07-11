using Kooboo.Commerce.Data.Folders.Disk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Folders
{
    public abstract class DataFolderFactory
    {
        public abstract DataFolder GetFolder(string virtualPath, IDataFileFormat defaultFileFormat);

        public abstract DataFile GetFile(string virtualPath, IDataFileFormat format);

        public static DataFolderFactory Current = new DiskDataFolderFactory();
    }
}
