using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Accessories
{
    public class AccessoriesTabModel
    {
        public int ProductId { get; set; }

        public IList<ProductAccessory> Accessories { get; set; }

        public AccessoriesTabModel()
        {
            Accessories = new List<ProductAccessory>();
        }
    }
}