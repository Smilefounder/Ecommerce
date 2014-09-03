using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Commerce.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.Commerce.Countries
{
    public class Country
    {
        [Param]
        public int Id { get; set; }

        [Param, StringLength(50)]
        public string Name { get; set; }

        [Param, StringLength(2)]
        public string TwoLetterIsoCode { get; set; }

        [Param, StringLength(3)]
        public string ThreeLetterIsoCode { get; set; }

        [Param, StringLength(3)]
        public string NumericIsoCode { get; set; }
    }
}