using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Products
{
    /// <summary>
    /// product review
    /// to get response from buyer(customer) after the transaction is completed.
    /// </summary>
    public class ProductReview
    {
        /// <summary>
        /// review id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// product id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Ranking value from 1-5. 
        /// </summary>
        public int Ranking { get; set; }
        /// <summary>
        /// user's remarks
        /// </summary>
        public string Reviews { get; set; }
        /// <summary>
        /// the remarks need to be approved to avoid throwing malicious, politic sensitive, ad words out.
        /// </summary>
        public bool IsApproved { get; set; }
        /// <summary>
        /// approved at utc
        /// </summary>
        public DateTime? ApprovedAtUtc { get; set; }
        /// <summary>
        /// created at utc
        /// </summary>
        public DateTime CreatedAtUtc { get; set; }
        /// <summary>
        /// ensure this review is allowed to be shown in public
        /// </summary>
        public bool ShowToPublic { get; set; }
        /// <summary>
        /// order id
        /// </summary>
        public int? OrderId { get; set; }
        /// <summary>
        /// customer id
        /// </summary>
        public int CustommerId { get; set; }
    }
}
