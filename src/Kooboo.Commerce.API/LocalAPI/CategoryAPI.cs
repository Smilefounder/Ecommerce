using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Categories.Services;

namespace Kooboo.Commerce.API.LocalAPI
{
    [Dependency(typeof(ICategoryAPI), ComponentLifeStyle.Transient, Key = "LocalAPI")]
    public class CategoryAPI : ICategoryAPI
    {
        private ICategoryService _categoryService;

        public CategoryAPI(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            var categories = _categoryService.GetRootCategories();
            Stack<Category> st = new Stack<Category>();
            foreach(var c in categories)
            {
                st.Push(c);
            }
            while (st.Count > 0)
            {
                var p = st.Pop();
                p.Children = _categoryService.GetChildCategories(p.Id).ToList();
                foreach (var c in p.Children)
                    st.Push(c);
            }
            return categories;
        }

        public IEnumerable<Category> GetSubCategories(int parentCategoryId)
        {
            var categories = _categoryService.GetChildCategories(parentCategoryId);
            return categories;
        }

        public Category GetCategory(int categoryId, bool loadParents = false)
        {
            var category = _categoryService.GetById(categoryId);
            if (category != null && loadParents)
            {
                var p = category;
                while (p.Parent != null)
                {
                    p = p.Parent;
                }

            }
            return category;
        }
    }
}
