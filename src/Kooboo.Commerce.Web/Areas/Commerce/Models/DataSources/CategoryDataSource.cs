using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Web.Mvc;
using Kooboo.Globalization;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Categories;

namespace Kooboo.Commerce.Web.Areas.Commerce.Models.DataSources {

    public class CategoryDataSource : ISelectListDataSource {

        private readonly IRepository<Category> _repository;
        public CategoryDataSource(IRepository<Category> repository) {
            _repository = repository;
        }

        static bool IsParent(int id, Category child) {
            var ret = false;
            while (child.Parent != null) {
                if (child.Parent.Id == id) {
                    ret = true;
                    break;
                }
                child = child.Parent;
            }
            return ret;
        }

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null) {
            var allCategories = _repository.Query().ToArray();
            //var currentIdStr = requestContext.GetRequestValue("Id");
            //int? currentId = null;
            //if (!string.IsNullOrEmpty(currentIdStr)) {
            //    currentId = int.Parse(currentIdStr);
            //}

            var selectList = new List<SelectListItem>();
            Stack<Category> st = new Stack<Category>();
            foreach(var cate in allCategories)
            {
                if(cate.Parent == null)
                {
                    st.Push(cate);
                }
            }

            while(st.Count > 0)
            {
                var cate = st.Pop();
                if (cate.Parent != null)
                    cate.Name = string.Format("{0} >> {1}", cate.Parent.Name, cate.Name);
                selectList.Add(new SelectListItem()
                {
                    Text = cate.Name,
                    Value = cate.Id.ToString()
                });
                if (cate.Children != null && cate.Children.Count > 0)
                {
                    foreach(var c in cate.Children)
                    {
                        st.Push(c);
                    }
                }
            }

            selectList.Insert(0, new SelectListItem()
            {
                Text = "[None]".Localize(),
                Value = string.Empty
            });

            //var list = new List<Category>();
            //foreach (var item in allCategories) {
            //    if (!currentId.HasValue || (currentId.Value != item.Id && !IsParent(currentId.Value, item))) {
            //        list.Add(item);
            //    }
            //}

            //foreach (var item in list) {
            //    var category = item;
            //    string fullname = category.Name;
            //    while (category.Parent != null) {
            //        fullname = string.Format("{0} >> {1}", category.Name, fullname);
            //        category = category.Parent;
            //    }
            //    item.Name = fullname;
            //}

            //var selectList = list.Select(o => new SelectListItemEx() {
            //    Text = o.Name,
            //    Value = o.Id.ToString(),
            //    HtmlAttributes = new Dictionary<string, object>() { { "style", "min-width:100px;" } }
            //}).ToList();

            //selectList.Insert(0, new SelectListItemEx() {
            //    Text = "[None]".Localize(),
            //    Value = string.Empty,
            //    HtmlAttributes = new Dictionary<string, object>() { { "style", "min-width:100px;" } }
            //});

            return selectList;
        }
    }
}