using Kooboo.IO;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace Kooboo.Commerce.Data.Folders
{
    public class DataFolder
    {
        public string Name { get; private set; }

        public string VirtualPath { get; private set; }

        public string PhysicalPath { get; private set; }

        public bool Exists
        {
            get
            {
                return Directory.Exists(PhysicalPath);
            }
        }

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

        public DataFolder(string virtualPath)
            : this(virtualPath, JsonDataFileFormat.Instance)
        {
        }

        public DataFolder(string virtualPath, IDataFileFormat defaultFileFormat)
        {
            VirtualPath = virtualPath;
            PhysicalPath = HostingEnvironment.MapPath(virtualPath);
            Name = Path.GetFileName(PhysicalPath);
            _defaultFileFormat = defaultFileFormat;
        }

        public void Create()
        {
            Directory.CreateDirectory(PhysicalPath);
        }

        public IEnumerable<DataFolder> GetFolders(string pattern = null)
        {
            var directory = new DirectoryInfo(PhysicalPath);
            if (!directory.Exists)
            {
                yield break;
            }

            var subdirs = pattern == null ? directory.EnumerateDirectories() : directory.EnumerateDirectories(pattern, SearchOption.TopDirectoryOnly);
            foreach (var subdir in subdirs)
            {
                yield return GetDataFolder(subdir);
            }
        }

        public DataFolder GetFolder(string name)
        {
            return GetFolder(name, null);
        }

        public DataFolder GetFolder(string name, IDataFileFormat defaultFileFormat)
        {
            Require.NotNullOrEmpty(name, "name");

            return new DataFolder(UrlUtility.Combine(VirtualPath, name), defaultFileFormat ?? DefaultFileFormat);
        }

        protected virtual IDataFileFormat GetFolderDefaultFileFormat(DirectoryInfo folder)
        {
            return JsonDataFileFormat.Instance;
        }

        private DataFolder GetDataFolder(DirectoryInfo directory)
        {
            return new DataFolder(UrlUtility.Combine(VirtualPath, directory.Name), GetFolderDefaultFileFormat(directory));
        }

        public DataFile GetFile(string name)
        {
            return GetFile(name, null);
        }

        public DataFile GetFile(string name, IDataFileFormat format)
        {
            Require.NotNullOrEmpty(name, "name");
            return new DataFile(UrlUtility.Combine(VirtualPath, name), format ?? DefaultFileFormat);
        }

        public IEnumerable<DataFile> GetFiles(string pattern = null)
        {
            var directory = new DirectoryInfo(PhysicalPath);
            if (!directory.Exists)
            {
                yield break;
            }

            var files = pattern == null ? directory.EnumerateFiles() : directory.EnumerateFiles(pattern, SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                yield return GetDataFile(file);
            }
        }

        private DataFile GetDataFile(FileInfo file)
        {
            return new DataFile(UrlUtility.Combine(VirtualPath, file.Name), GetDataFileFormat(file));
        }

        protected virtual IDataFileFormat GetDataFileFormat(FileInfo file)
        {
            return DefaultFileFormat;
        }

        public DataFile AddFile(string name, object content)
        {
            return AddFile(name, content, DefaultFileFormat);
        }

        public DataFile AddFile(string name, object content, IDataFileFormat format)
        {
            Require.NotNullOrEmpty(name, "name");

            var file = new DataFile(UrlUtility.Combine(VirtualPath, name), format ?? DefaultFileFormat);
            file.Write(content);
            return file;
        }

        public void Delete()
        {
            if (Exists)
            {
                IOUtility.DeleteDirectory(PhysicalPath, true);
            }
        }
    }
}
