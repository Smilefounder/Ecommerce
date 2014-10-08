using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Events
{
    public class LanguageDeleted : IEvent
    {
        public string Name { get; set; }
    }
}