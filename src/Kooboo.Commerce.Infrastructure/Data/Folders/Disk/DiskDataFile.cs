using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Kooboo.Commerce.Data.Folders.Disk
{
    public class DiskDataFile : DataFile
    {
        public string PhysicalPath { get; private set; }

        public override bool Exists
        {
            get
            {
                return File.Exists(PhysicalPath);
            }
        }

        public DiskDataFile(string virtualPath, IDataFileFormat format)
            : base(virtualPath, format)
        {
            PhysicalPath = HostingEnvironment.MapPath(virtualPath);
        }
        
        public override object Read(Type type)
        {
            if (!Exists)
            {
                return null;
            }

            var content = File.ReadAllText(PhysicalPath, Encoding.UTF8);
            return Format.Deserialize(content, type);
        }

        public override void Write(object content)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(PhysicalPath));

            var text = Format.Serialize(content);
            File.WriteAllText(PhysicalPath, text);
        }

        public override void Delete()
        {
            if (Exists)
            {
                File.Delete(PhysicalPath);
            }
        }
    }
}
