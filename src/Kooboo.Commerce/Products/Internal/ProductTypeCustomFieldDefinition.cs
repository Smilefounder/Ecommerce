using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kooboo.Commerce.Products.Internal
{
    public class ProductTypeCustomFieldDefinition
    {
        [Key, Column(Order = 0)]
        public int ProductTypeId { get; set; }

        [Key, Column(Order = 1)]
        public int CustomFieldId { get; set; }

        public virtual ProductType ProductType { get; set; }

        public virtual CustomFieldDefinition CustomField { get; set; }

        public int Sequence { get; set; }
    }
}
