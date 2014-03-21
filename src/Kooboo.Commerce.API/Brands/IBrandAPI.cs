using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Brands
{
    public interface IBrandAPI : IBrandQuery
    {
        IBrandQuery Query();
    }
}
