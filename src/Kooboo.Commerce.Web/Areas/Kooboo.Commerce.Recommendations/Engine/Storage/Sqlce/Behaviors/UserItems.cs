using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Behaviors
{
    public class UserItems
    {
        [Key, StringLength(50)]
        public string UserId { get; set; }

        public string ItemIds { get; set; }
    }
}