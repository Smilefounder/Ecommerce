using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Web.Areas.Commerce.Models.DataSources;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Categories
{
    public class CategoryEditorModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        public string Photo { get; set; }

        public string Description { get; set; }

        [UIHint("DropDownList")]
        [DataSource(typeof(CategoryDataSource))]
        [DisplayName("Parent Category")]
        public int? ParentId { get; set; }

        public ICollection<NameValue> CustomFields { get; set; }

        public CategoryEditorModel()
        {
            CustomFields = new List<NameValue>();
        }
    }
}