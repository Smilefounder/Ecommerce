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

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.Categories {

    public class CategoryEditorModel {

        public CategoryEditorModel() {
            this.Children = new List<CategoryEditorModel>();
        }

        public CategoryEditorModel(Category category, bool children = false)
            : this() {
            this.Id = category.Id;
            this.Name = category.Name;
            this.Photo = category.Photo;
            this.Description = category.Description;
            this.Published = category.Published;
            //
            if (category.Parent != null) {
                this.ParentId = category.Parent.Id.ToString();
            }
            //
            if (children && category.Children != null) {
                foreach (var item in category.Children) {
                    this.Children.Add(new CategoryEditorModel(item, children));
                }
            }
        }

        public void UpdateTo(Category category) {
            category.Name = (this.Name ?? string.Empty).Trim();
            category.Photo = (this.Photo ?? string.Empty).Trim();
            category.Description = (this.Description ?? string.Empty).Trim();
            category.Published = this.Published;
            //
            //if (!string.IsNullOrEmpty(this.ParentId)) {
            //    category.Parent = new Category() { Id = int.Parse(this.ParentId) };
            //}
            //
            if (this.Children != null) {
                category.Children = new List<Category>();
                foreach (var item in this.Children) {
                    var obj = new Category();
                    item.UpdateTo(obj);
                    category.Children.Add(obj);
                }
            }
        }

        public int Id {
            get;
            set;
        }

        [Required(ErrorMessage = "Required")]
        public string Name {
            get;
            set;
        }

        public string Photo {
            get;
            set;
        }

        public string Description {
            get;
            set;
        }

        public bool Published {
            get;
            set;
        }

        [UIHint("DropDownList")]
        [DataSource(typeof(CategoryDataSource))]
        [DisplayName("Parent Category")]
        public string ParentId {
            get;
            set;
        }

        public List<CategoryEditorModel> Children {
            get;
            set;
        }
    }
}