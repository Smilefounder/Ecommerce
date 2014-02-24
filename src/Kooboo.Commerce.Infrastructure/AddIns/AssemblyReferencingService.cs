using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Kooboo.Commerce.AddIns
{
    public class AssemblyReferencingService : IAssemblyReferencingService
    {
        private string _filePath;
        private ReaderWriterLockSlim _readerWriterLock = new ReaderWriterLockSlim();

        public AssemblyReferencingService()
            : this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\AssemblyReferences.txt"))
        {
        }

        public AssemblyReferencingService(string filePath)
        {
            Require.NotNullOrEmpty(filePath, "filePath");
            _filePath = filePath;
        }

        public IEnumerable<AssemblyConflict> GetAssemlbyConflicts(IEnumerable<AssemblyInfo> assemblyInfos)
        {
            EnsureInitialized();

            var conflicts = new List<AssemblyConflict>();
            var references = LoadAssemblyReferencesWithLock();

            foreach (var assemblyToCheck in assemblyInfos)
            {
                var reference = FindAssemblyReferences(references, assemblyToCheck.AssemblyName);
                if (reference != null && reference.Version != assemblyToCheck.Version)
                {
                    conflicts.Add(new AssemblyConflict(assemblyToCheck.AssemblyName, reference.Version, assemblyToCheck.Version));
                }
            }

            return conflicts;
        }

        public IEnumerable<AssemblyInfo> GetReferencedAssemblies(string addInId)
        {
            EnsureInitialized();

            var references = LoadAssemblyReferencesWithLock();

            var assemblies = from reference in references
                             where reference.ReferecingAddIns.Contains(addInId)
                             select new AssemblyInfo(reference.AssemblyName, reference.Version);

            return assemblies.ToList();
        }

        public IEnumerable<string> GetReferencingAddIns(AssemblyInfo assemblyInfo)
        {
            EnsureInitialized();

            var references = LoadAssemblyReferencesWithLock();
            var reference = FindAssemblyReferences(references, assemblyInfo.AssemblyName);
            if (reference == null)
            {
                return Enumerable.Empty<string>();
            }

            return reference.ReferecingAddIns.ToList();
        }

        public void AddReference(AssemblyInfo assemblyInfo, string addInId)
        {
            Require.NotNull(assemblyInfo, "assemblyInfo");
            Require.NotNullOrEmpty(addInId, "addInId");

            EnsureInitialized();

            try
            {
                _readerWriterLock.EnterWriteLock();

                var references = LoadAssemblyReferencesNoLock();
                var reference = FindAssemblyReferences(references, assemblyInfo.AssemblyName);
                if (reference == null)
                {
                    reference = new AssemblyReferences
                    {
                        AssemblyName = assemblyInfo.AssemblyName,
                        Version = assemblyInfo.Version
                    };
                    references.Add(reference);
                }

                reference.ReferecingAddIns.Add(addInId);

                SaveAssemblyReferencesNoLock(references);
            }
            finally
            {
                _readerWriterLock.ExitWriteLock();
            }
        }

        public bool RemoveReference(AssemblyInfo assemblyInfo, string addInId)
        {
            Require.NotNull(assemblyInfo, "assemblyInfo");
            Require.NotNullOrEmpty(addInId, "addInId");

            EnsureInitialized();

            try
            {
                _readerWriterLock.EnterWriteLock();

                var references = LoadAssemblyReferencesNoLock();
                var reference = FindAssemblyReferences(references, assemblyInfo.AssemblyName);
                if (reference != null)
                {
                    var removed = reference.ReferecingAddIns.Remove(addInId);
                    if (removed)
                    {
                        SaveAssemblyReferencesNoLock(references);
                    }

                    return removed;
                }

                return false;
            }
            finally
            {
                _readerWriterLock.ExitWriteLock();
            }
        }

        private List<AssemblyReferences> LoadAssemblyReferencesWithLock()
        {
            try
            {
                _readerWriterLock.EnterReadLock();
                return LoadAssemblyReferencesNoLock();
            }
            finally
            {
                _readerWriterLock.ExitReadLock();
            }
        }

        private List<AssemblyReferences> LoadAssemblyReferencesNoLock()
        {
            var data = Kooboo.IO.IOUtility.ReadAsString(_filePath);
            return JsonConvert.DeserializeObject<List<AssemblyReferences>>(data);
        }

        private void EnsureInitialized()
        {
            if (IsInitialized()) return;

            try
            {
                _readerWriterLock.EnterWriteLock();

                if (!IsInitialized())
                {
                    var references = new List<AssemblyReferences>();

                    var binFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin");
                    foreach (var dllFile in Directory.EnumerateFiles(binFolder, "*.dll"))
                    {
                        var asmName = AssemblyName.GetAssemblyName(dllFile);
                        var reference = new AssemblyReferences
                        {
                            AssemblyName = asmName.Name,
                            Version = asmName.Version.ToString()
                        };
                        reference.ReferecingAddIns.Add("System");

                        references.Add(reference);
                    }

                    SaveAssemblyReferencesNoLock(references);
                }
            }
            finally
            {
                _readerWriterLock.ExitWriteLock();
            }
        }

        private bool IsInitialized()
        {
            return File.Exists(_filePath);
        }

        private void SaveAssemblyReferencesNoLock(IEnumerable<AssemblyReferences> references)
        {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(references), Encoding.UTF8);
        }

        private AssemblyReferences FindAssemblyReferences(IEnumerable<AssemblyReferences> references, string assemblyName)
        {
            return references.FirstOrDefault(x => x.AssemblyName.Equals(assemblyName, StringComparison.OrdinalIgnoreCase));
        }

        class AssemblyReferences
        {
            public string AssemblyName { get; set; }

            public string Version { get; set; }

            public ISet<string> ReferecingAddIns { get; set; }

            public AssemblyReferences()
            {
                ReferecingAddIns = new HashSet<string>();
            }
        }
    }
}
