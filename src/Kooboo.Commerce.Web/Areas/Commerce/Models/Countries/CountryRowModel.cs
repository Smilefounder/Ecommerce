using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Countries;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.Commerce.Web.Framework.UI.Grid2;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Countries
{
    [Grid(IdProperty = "Id", Checkable = true)]
    public class CountryRowModel
    {
        public int Id { get; set; }

        [LinkColumn("Edit")]
        public string Name { get; set; }

        [GridColumn(HeaderText = "Three letter ISO code")]
        public string ThreeLetterIsoCode { get; set; }

        [GridColumn(HeaderText = "Two letter ISO code")]
        public string TwoLetterIsoCode { get; set; }

        [GridColumn(HeaderText = "Numeric ISO code")]
        public string NumericIsoCode { get; set; }

        public CountryRowModel()
        {
        }

        public CountryRowModel(Country country)
        {
            Id = country.Id;
            Name = country.Name;
            ThreeLetterIsoCode = country.ThreeLetterIsoCode;
            TwoLetterIsoCode = country.TwoLetterIsoCode;
            NumericIsoCode = country.NumericIsoCode;
        }
    }
}