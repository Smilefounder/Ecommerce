﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Products
{
    public interface IProductApi
    {
        Query<Product> Query();
    }
}
