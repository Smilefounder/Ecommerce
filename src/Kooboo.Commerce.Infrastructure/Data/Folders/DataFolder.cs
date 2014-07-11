using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Folders
{
    public abstract class DataFolder
    {
        protected DataFolder(string virtualPath, IDataFileFormat defaultFileFormat)
        {
            Name = Path.GetFileName(virtualPath);
            VirtualPath = virtualPath;
            _defaultFileFormat = defaultFileFormat;
        }

        public string Name { get; protected set; }

        public string VirtualPath { get; protected set; }

        public abstract bool Exists { get; }

        private IDataFileFormat _defaultFileFormat;

        public IDataFileFormat DefaultFileFormat
        {
            get
            {
                return _defaultFileFormat;
            }
            set
            {
                Require.NotNull(value, "value");
                _defaultFileFormat = value;
            }
        }

        public abstract void Create();

        public abstract IEnumerable<DataFolder> GetFolders(string searchPattern = null);

        public DataFolder GetFolder(string name)
        {
            return GetFolder(name, DefaultFileFormat);
        }

        public abstract DataFolder GetFolder(string name, IDataFileFormat fileFormat);

        public DataFile GetFile(string name)
        {
            return GetFile(name, DefaultFileFormat);
        }

        public abstract DataFile GetFile(string name, IDataFileFormat format);

        public abstract IEnumerable<DataFile> GetFiles(string searchPattern = null);

        public abstract void Delete();
    }
}
