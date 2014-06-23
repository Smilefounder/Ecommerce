﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Recommendations
{
    public class ProductRecommendation
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string BrandName { get; set; }

        public int Rank { get; set; }
    }
}