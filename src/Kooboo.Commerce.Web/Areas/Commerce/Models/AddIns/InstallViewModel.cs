using Kooboo.Commerce.AddIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.AddIns
{
    public class InstallViewModel
    {
        public bool HasUploadedPackage { get; set; }

        public bool IsMigration { get; set; }

        public AddInMeta AddInMeta { get; set; }

        public AddInMeta CurrentAddInMeta { get; set; }

        public List<string> Errors { get; set; }

        public List<AssemblyConflict> AssemblyConflicts { get; set; }

        public List<AssemblyConflictSolution> AssemblyConflictSolutions { get; set; }

        public InstallViewModel()
        {
            Errors = new List<string>();
            AssemblyConflicts = new List<AssemblyConflict>();
            AssemblyConflictSolutions = new List<AssemblyConflictSolution>();
        }
    }
}