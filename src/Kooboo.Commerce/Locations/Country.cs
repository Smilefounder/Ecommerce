using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Locations
{
    public class Country
    {
        [Param]
        public int Id { get; set; }

        [Param]
        public string Name { get; set; }

        [Param]
        public string ThreeLetterISOCode { get; set; }

        [Param]
        public string TwoLetterISOCode { get; set; }

        [Param]
        public string NumericISOCode { get; set; }
    }
}