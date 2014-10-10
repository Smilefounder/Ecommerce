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

        public override int Write(Stream stream)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(PhysicalPath));

            var length = 0;

            using (var fs = new FileStream(PhysicalPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var buffer = new byte[2048];

                while (true)
                {
                    var count = stream.Read(buffer, 0, buffer.Length);
                    if (count > 0)
                    {
                        length += count;
                        fs.Write(buffer, 0, count);
                    }
                    else
                    {
                        break;
                    }
                }

                fs.Flush();
            }

            return length;
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
