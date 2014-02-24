using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.AddIns
{
    public class AddInPreparationResult
    {
        public bool IsValid { get; set; }

        public IList<string> Errors { get; set; }

        public IList<AssemblyConflict> AssemblyConflicts { get; set; }

        public AddInPreparationResult()
        {
            Errors = new List<string>();
            AssemblyConflicts = new List<AssemblyConflict>();
        }
    }
}
