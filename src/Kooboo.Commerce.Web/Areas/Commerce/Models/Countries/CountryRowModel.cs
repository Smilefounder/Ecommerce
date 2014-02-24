using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Locations;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Countries
{
    [Grid(IdProperty = "Id", Checkable = true)]
    public class CountryRowModel
    {
        public int Id { get; set; }

        [GridColumn(GridItemColumnType = typeof (EditGridActionItemColumn))]
        public string Name { get; set; }
        [GridColumn()]
        public string ThreeLetterISOCode { get; set; }

        [GridColumn()]
        public string TwoLetterISOCode { get; set; }

        [GridColumn()]
        public string NumericISOCode { get; set; }

        public CountryRowModel()
        {
        }

        public CountryRowModel(Country country)
        {
            Id = country.Id;
            Name = country.Name;
            ThreeLetterISOCode = country.ThreeLetterISOCode;
            TwoLetterISOCode = country.TwoLetterISOCode;
            NumericISOCode = country.NumericISOCode;
        }
    }
}