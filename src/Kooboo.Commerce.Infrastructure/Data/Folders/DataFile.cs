using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Kooboo.Commerce.Data.Folders
{
    public class DataFile
    {
        public string Name { get; private set; }

        public string VirtualPath { get; private set; }

        public string PhysicalPath { get; private set; }

        private IDataFileFormat _format;

        public IDataFileFormat Format
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

        public bool Exists
        {
            get
            {
                return File.Exists(PhysicalPath);
            }
        }

        public DataFile(string virtualPath, IDataFileFormat format)
        {
            VirtualPath = virtualPath;
            PhysicalPath = HostingEnvironment.MapPath(virtualPath);
            Name = Path.GetFileName(PhysicalPath);
            _format = format;
        }

        public T Read<T>()
        {
            var content = Read(typeof(T));
            return content == null ? default(T) : (T)content;
        }

        public virtual object Read(Type type)
        {
            if (!Exists)
            {
                return null;
            }

            var content = File.ReadAllText(PhysicalPath, Encoding.UTF8);
            return Format.Deserialize(content, type);
        }

        public virtual void Write(object content)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(PhysicalPath));

            var text = Format.Serialize(content);
            File.WriteAllText(PhysicalPath, text);
        }

        public void Delete()
        {
            if (Exists)
            {
                File.Delete(PhysicalPath);
            }
        }
    }
}
