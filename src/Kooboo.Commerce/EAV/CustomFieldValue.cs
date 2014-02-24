using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.EAV
{
    public class CustomFieldValue
    {
        public int Id { get; set; }

        public string EntityType { get; set; }

        public string EntityKey { get; set; }

        public string Value { get; set; }
    }
}
