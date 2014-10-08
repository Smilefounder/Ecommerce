using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Events
{
    public class LanguageAdded : IEvent
    {
        public string Name { get; set; }
    }
}