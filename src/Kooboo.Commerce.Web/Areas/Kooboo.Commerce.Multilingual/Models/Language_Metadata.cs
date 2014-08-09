using Kooboo.Commerce.Web.Framework.UI.Grid2;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Multilingual.Models
{
    [MetadataFor(typeof(Language))]
    [Grid(Checkable = true, IdProperty = "Name")]
    public class Language_Metadata
    {
        [Required]
        [Display(Name = "Language")]
        [UIHint("DropDownList")]
        [DataSource(typeof(LangaugeSelectListDataSource))]
        [LinkColumn("Edit", HeaderText = "Code")]
        public string Name { get; set; }

        [Required]
        [GridColumn]
        public string DisplayName { get; set; }
    }

    public class LangaugeSelectListDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            return CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                              .Select(x => new SelectListItem
                              {
                                  Text = x.NativeName,
                                  Value = x.Name
                              })
                              .ToList();
        }
    }

}