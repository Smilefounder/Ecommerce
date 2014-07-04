using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Commerce.Rules.Activities.Simple
{
    public class SimpleActivityConfig
    {
        [Required]
        public string Parameter1 { get; set; }

        public int Parameter2 { get; set; }
    }
}
