using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Plugins.Vitaminstore
{
    public class NewAddressModel
    {
        public string Postcode { get; set; }

        public string HouseNumber { get; set; }

        public string HouseNumberAddition { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public int Country { get; set; }
    }
}
