using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Api.Countries
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ThreeLetterIsoCode { get; set; }

        public string TwoLetterIsoCode { get; set; }

        public string NumericIsoCode { get; set; }
    }
}
