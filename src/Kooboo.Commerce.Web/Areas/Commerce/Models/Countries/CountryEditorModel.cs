using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Countries;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Countries
{
    [Grid]
    public class CountryEditorModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [Display(Name = "Three letter ISO code")]
        public string ThreeLetterIsoCode { get; set; }

        [Display(Name = "Two letter ISO code")]
        public string TwoLetterIsoCode { get; set; }

        [Display(Name = "Numeric ISO code")]
        public string NumericIsoCode { get; set; }

        public CountryEditorModel()
        {
        }

        public CountryEditorModel(Country country)
        {
            Id = country.Id;
            Name = country.Name;
            ThreeLetterIsoCode = country.ThreeLetterIsoCode;
            TwoLetterIsoCode = country.TwoLetterIsoCode;
            NumericIsoCode = country.NumericIsoCode;
        }

        public void UpdateTo(Country country)
        {
            country.Id = Id;
            country.Name = Name.Trim();
            country.ThreeLetterIsoCode = ThreeLetterIsoCode;
            country.TwoLetterIsoCode = TwoLetterIsoCode;
            country.NumericIsoCode = NumericIsoCode;
        }
    }
}