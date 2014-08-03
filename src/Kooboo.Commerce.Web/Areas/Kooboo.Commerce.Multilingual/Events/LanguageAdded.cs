using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Events
{
    public class LanguageAdded : Event
    {
        public string Name { get; private set; }

        public LanguageAdded(string name)
        {
            Name = name;
        }
    }
}