using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations.Engine.Storage.Sqlce.Collaborative
{
    public class ItemSimilarity
    {
        [Key, Column(Order = 0)]
        public string Item1 { get; set; }

        [Key, Column(Order = 1)]
        public string Item2 { get; set; }

        public double Similarity { get; set; }
    }
}