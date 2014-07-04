using Kooboo.Commerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Instances
{
    public class CommerceInstanceModel
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string DbProvider { get; set; }
    }
}