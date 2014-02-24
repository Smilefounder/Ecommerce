using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Commerce.Catalogs;

namespace Kooboo.Commerce.Stores
{
    public class Store
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Catalog Catalog { get; set; }
    }
}