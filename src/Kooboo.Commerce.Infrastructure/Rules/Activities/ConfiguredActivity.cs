using Kooboo.Commerce.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Rules.Activities
{
    public class ConfiguredActivity
    {
        public string ActivityName { get; set; }

        public string Description { get; set; }

        public string Config { get; set; }

        public bool Async { get; set; }

        public int AsyncDelay { get; set; }
    }
}
