using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Data.Folders
{
    public abstract class DataFile
    {
        protected DataFile(string virtualPath, IDataFileFormat format)
        {
            Name = Path.GetFileName(virtualPath);
            VirtualPath = virtualPath;
            Format = format;
        }

        public string Name { get; protected set; }

        public string VirtualPath { get; protected set; }

        private IDataFileFormat _format;

        public virtual IDataFileFormat Format
        {
            get
            {
                return _format;
            }
            set
            {
                Require.NotNull(value, "value");
                _format = value;
            }
        }

        public abstract bool Exists { get; }

        public T Read<T>()
        {
            var content = Read(typeof(T));
            return content == null ? default(T) : (T)content;
        }

        public abstract object Read(Type type);

        public abstract void Write(object content);

        public abstract void Delete();
    }

    public static class DataFileExtensions
    {
        public static DataFile Cached(this DataFile file)
        {
            if (file is CachedDataFile)
            {
                return file;
            }

            return new CachedDataFile(file);
        }
    }
}
