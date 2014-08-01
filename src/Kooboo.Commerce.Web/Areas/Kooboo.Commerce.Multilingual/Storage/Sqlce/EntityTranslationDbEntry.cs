using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Storage.Sqlce
{
    public class EntityTranslationDbEntry
    {
        [Key, Column(Order = 0), StringLength(10)]
        public string Culture { get; set; }

        [Key, Column(Order = 1), StringLength(300)]
        public string EntityType { get; set; }

        [Key, Column(Order = 2), StringLength(50)]
        public string EntityKey { get; set; }

        public bool IsOutOfDate { get; set; }

        /// <summary>
        /// Property translations as json.
        /// </summary>
        public string Properties { get; set; }
    }
}