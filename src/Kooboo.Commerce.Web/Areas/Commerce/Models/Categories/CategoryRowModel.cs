using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Web.Grid2;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Categories
{

    [Grid(Checkable = true, IdProperty = "Id")]
    public class CategoryRowModel
    {

        public CategoryRowModel()
        {
        }

        public CategoryRowModel(Category category, bool parent = false)
            : this()
        {
            this.Id = category.Id;
            this.Name = category.Name;
            this.Photo = category.Photo;
            this.Description = category.Description;
            this.Published = category.Published;
            this.ChildrenCount = category.Children == null ? 0 : category.Children.Count;
            if (parent && category.Parent != null)
            {
                this.Parent = new CategoryRowModel(category.Parent, parent);
            }
        }

        public int Id
        {
            get;
            set;
        }

        [GridColumn(GridItemColumnType = typeof(EditGridActionItemColumn))]
        public string Name
        {
            get;
            set;
        }

        [GridColumn]
        public string Photo
        {
            get;
            set;
        }

        [GridColumn]
        public string Description
        {
            get;
            set;
        }

        [BooleanGridColumn]
        public bool Published
        {
            get;
            set;
        }

        public int ChildrenCount { get; set; }

        public CategoryRowModel Parent
        {
            get;
            set;
        }

        //[ActionGridColumn(HeaderText = "Add Child", ButtonText = "Add Child", ActionName = "AddChild", ImageSrc = "")]
        //public string _AddChild {
        //    get;
        //    set;
        //}
    }
}