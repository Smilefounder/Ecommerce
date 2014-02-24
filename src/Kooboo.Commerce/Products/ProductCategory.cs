using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kooboo.Commerce.Categories;

namespace Kooboo.Commerce.Products
{
    public class ProductCategory
    {
        [Key, Column(Order = 0)]
        public int ProductId { get; set; }
        [Key, Column(Order = 1)]
        public int CategoryId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Category Category { get; set; }
    }
}
