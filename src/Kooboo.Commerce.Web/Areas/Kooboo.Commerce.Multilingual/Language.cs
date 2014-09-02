using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual
{
    public class Language
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public Language Clone()
        {
            return (Language)base.MemberwiseClone();
        }
    }
}