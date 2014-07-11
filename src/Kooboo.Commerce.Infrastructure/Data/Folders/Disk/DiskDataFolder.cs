using Kooboo.IO;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Kooboo.Commerce.Data.Folders.Disk
{
    public class DiskDataFolder : DataFolder
    {
        public string PhysicalPath { get; private set; }

        public override bool Exists
        {
            get
            {
                return Directory.Exists(PhysicalPath);
            }
        }

        public DiskDataFolder(string virtualPath, IDataFileFormat defaultFileFormat)
            : base(virtualPath, defaultFileFormat)
        {
            PhysicalPath = HostingEnvironment.MapPath(virtualPath);
        }

        public override void Create()
        {
            Directory.CreateDirectory(PhysicalPath);
        }

        public override IEnumerable<DataFolder> GetFolders(string searchPattern = null)
        {
            var directory = new DirectoryInfo(PhysicalPath);
            if (!directory.Exists)
            {
                yield break;
            }

            var subdirs = searchPattern == null ? directory.EnumerateDirectories() : directory.EnumerateDirectories(searchPattern, SearchOption.TopDirectoryOnly);
            foreach (var subdir in subdirs)
            {
                yield return new DiskDataFolder(UrlUtility.Combine(VirtualPath, subdir.Name), DefaultFileFormat);
            }
        }

        public override DataFolder GetFolder(string name, IDataFileFormat defaultFileFormat)
        {
            Require.NotNullOrEmpty(name, "name");
            return new DiskDataFolder(UrlUtility.Combine(VirtualPath, name), defaultFileFormat ?? DefaultFileFormat);
        }

        public override DataFile GetFile(string name, IDataFileFormat format)
        {
            Require.NotNullOrEmpty(name, "name");
            return new DiskDataFile(UrlUtility.Combine(VirtualPath, name), format ?? DefaultFileFormat);
        }

        public override IEnumerable<DataFile> GetFiles(string searchPattern = null)
        {
            var directory = new DirectoryInfo(PhysicalPath);
            if (!directory.Exists)
            {
                yield break;
            }

            var files = searchPattern == null ? directory.EnumerateFiles() : directory.EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                yield return new DiskDataFile(UrlUtility.Combine(VirtualPath, file.Name), DefaultFileFormat);
            }
        }

        public override void Delete()
        {
            if (Exists)
            {
                IOUtility.DeleteDirectory(PhysicalPath, true);
            }
        }
    }
}
