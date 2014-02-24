using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Commerce.AddIns
{
    public class AddInFolder
    {
        private AddInMeta _meta;

        public AddInMeta Meta
        {
            get
            {
                if (_meta == null)
                {
                    _meta = AddInMeta.LoadFrom(MetaPath.PhysicalPath);
                }

                return _meta;
            }
        }

        public PathInfo Path { get; private set; }

        public PathInfo MetaPath
        {
            get
            {
                return PathInfo.Combine(Path, AddInMeta.FileName);
            }
        }

        public PathInfo BinPath
        {
            get
            {
                return PathInfo.Combine(Path, "Bin");
            }
        }

        public AddInFolder(PathInfo path)
        {
            Require.NotNull(path, "path");
            Path = path;
        }

        public static bool IsAddInFolder(string path)
        {
            var metaPath = System.IO.Path.Combine(path, AddInMeta.FileName);
            return File.Exists(metaPath);
        }

        public IList<ValidationResult> Validate()
        {
            var results = new List<ValidationResult>();

            if (!File.Exists(MetaPath.PhysicalPath))
            {
                results.Add(new ValidationResult("Missing meta.config file."));
            }

            AddInMeta meta = null;

            try
            {
                meta = AddInMeta.LoadFrom(MetaPath.PhysicalPath);
            }
            catch (Exception ex)
            {
                results.Add(new ValidationResult("Cannot load meta.config file, " + ex.Message));
            }

            results.AddRange(meta.Validate());

            return results;
        }

        public IEnumerable<AssemblyInfo> GetAssemblies()
        {
            foreach (var dllFile in Directory.EnumerateFiles(BinPath.PhysicalPath, "*.dll"))
            {
                var asmName = AssemblyName.GetAssemblyName(dllFile);
                yield return new AssemblyInfo(asmName.Name, asmName.Version.ToString());
            }
        }

        public void CopyTo(PathInfo destination)
        {
            Kooboo.IO.IOUtility.CopyDirectory(Path.PhysicalPath, destination.PhysicalPath);
        }
    }
}
