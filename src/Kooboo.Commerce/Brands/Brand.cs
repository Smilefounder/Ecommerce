﻿using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Brands
{
    public class Brand
    {
        [ConditionParameter(Name = "BrandId", DisplayName = "Brand ID")]
        public int Id { get; set; }

        [ConditionParameter(Name = "BrandName", DisplayName = "Brand Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// The logo file path.
        /// </summary>
        public string Logo { get; set; }
    }
}