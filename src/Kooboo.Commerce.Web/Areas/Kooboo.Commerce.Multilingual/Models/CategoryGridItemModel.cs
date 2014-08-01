using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Multilingual.Models
{
    public class CategoryGridItemModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string TranslatedName { get; set; }

        public bool IsTranslated { get; set; }

        public bool IsOutOfDate { get; set; }

        public int Level { get; set; }

        public int ChildrenCount { get; set; }

        public bool Expanded { get; set; }

        public IList<CategoryGridItemModel> Children { get; set; }

        public CategoryGridItemModel()
        {
            Children = new List<CategoryGridItemModel>();
        }
    }
}