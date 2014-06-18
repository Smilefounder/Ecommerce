using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce
{
    public partial class ExtendedQuery
    {
        public interface ProductQuery : IExtendedQuery<Products.ProductQueryModel>
        { }
    }
}
