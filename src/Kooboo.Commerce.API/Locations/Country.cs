using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.API.Locations
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ThreeLetterISOCode { get; set; }

        public string TwoLetterISOCode { get; set; }

        public string NumericISOCode { get; set; }
    }
}
