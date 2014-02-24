using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Locations;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Countries
{
    [Grid]
    public class CountryEditorModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        public string ThreeLetterISOCode { get; set; }

        public string TwoLetterISOCode { get; set; }

        public string NumericISOCode { get; set; }

        public CountryEditorModel()
        {
        }

        public CountryEditorModel(Country country)
        {
            Id = country.Id;
            Name = country.Name;
            ThreeLetterISOCode = country.ThreeLetterISOCode;
            TwoLetterISOCode = country.TwoLetterISOCode;
            NumericISOCode = country.NumericISOCode;
        }

        public void UpdateTo(Country country)
        {
            country.Id = Id;
            country.Name = Name.Trim();
            country.ThreeLetterISOCode = ThreeLetterISOCode;
            country.TwoLetterISOCode = TwoLetterISOCode;
            country.NumericISOCode = NumericISOCode;
        }
    }
}