using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Behaviors
{
    public class ItemUsers
    {
        [Key, StringLength(50)]
        public string ItemId { get; set; }

        public string UserIds { get; set; }
    }
}