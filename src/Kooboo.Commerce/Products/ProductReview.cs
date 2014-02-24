using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products
{
    public class ProductReview
    {
        public virtual int Id { get; set; }

        public virtual int ProductId { get; set; }
        /// <summary>
        /// Ranking value from 1-5. 
        /// </summary>
        public virtual int Ranking { get; set; }

        public virtual string Reviews { get; set; }

        public virtual bool IsApproved { get; set; }

        public virtual DateTime? ApprovedAtUtc { get; set; }

        public virtual DateTime CreatedAtUtc { get; set; }

        public virtual bool ShowToPublic { get; set; }

        public virtual int? OrderId { get; set; }

        public virtual int CustommerId { get; set; }
    }
}
