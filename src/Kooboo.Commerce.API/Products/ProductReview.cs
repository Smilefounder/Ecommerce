using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Products
{
    public class ProductReview
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        /// <summary>
        /// Ranking value from 1-5. 
        /// </summary>
        public int Ranking { get; set; }

        public string Reviews { get; set; }

        public bool IsApproved { get; set; }

        public DateTime? ApprovedAtUtc { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public bool ShowToPublic { get; set; }

        public int? OrderId { get; set; }

        public int CustommerId { get; set; }
    }
}
