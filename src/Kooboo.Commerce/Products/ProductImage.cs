using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Data.Events;
using Kooboo.Commerce.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Products
{
    public class ProductImage : IOrphanable
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Size { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        [Column]
        protected int? ProductId { get; set; }

        bool IOrphanable.IsOrphan()
        {
            return ProductId == null;
        }
    }
}
