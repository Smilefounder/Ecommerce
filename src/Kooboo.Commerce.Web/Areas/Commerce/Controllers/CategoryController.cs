using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Commerce.Data;
using Kooboo.Commerce.Web.Areas.Commerce.Models.Categories;
using Kooboo.Commerce.Categories;
using Kooboo.Commerce.Categories.Services;
using Kooboo.Commerce.Web.Mvc;
using Kooboo.Commerce.Web.Framework.Mvc;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{

    public class CategoryController : CommerceControllerBase
    {

        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ActionResult Index(int? page, int? pageSize)
        {
            var categories = _categoryService.Query().Where(o => o.Parent == null)
                .OrderByDescending(x => x.Id)
                .Select(x => new CategoryRowModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Published = x.Published,
                    ChildrenCount = x.Children.Count
                })
                .ToPagedList(page, pageSize);
                //.Transform(x => new CategoryRowModel(x, true));

            //foreach (var item in categories)
            //{
            //    var category = item;
            //    while (category.Parent != null)
            //    {
            //        item.Name = string.Format("{0} >> {1}", category.Parent.Name, item.Name);
            //        category = category.Parent;
            //    }
            //}

            // ret
            return View(categories);
        }

        public ActionResult Children(int parentId)
        {
            var children = _categoryService.Query().Where(o => o.Parent.Id == parentId)
                .Select(x => new CategoryRowModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Published = x.Published,
                    ChildrenCount = x.Children.Count
                })
                .ToArray();
            return JsonNet(children);
        }

        public ActionResult Create(int? parentId)
        {
            var model = new CategoryEditorModel();
            if (parentId.HasValue)
            {
                model.ParentId = parentId.Value.ToString();
                var parent = _categoryService.GetById(parentId.Value);
                model.SetParentCrumble(parent);
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var category = _categoryService.GetById(id);
            var model = new CategoryEditorModel(category);
            return View(model);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Save(CategoryEditorModel model, string @return)
        {
            model.CustomFields = FormHelper.BindToModels<CategoryCustomFieldModel>(Request.Form, "CustomFields.");

            var category = new Category();
            var parentId = Request.RequestContext.GetRequestValue("ParentId");
            if (!string.IsNullOrEmpty(parentId))
            {
                category.Parent = _categoryService.GetById(int.Parse(parentId));
            }

            model.UpdateTo(category);

            if (model.Id > 0)
            {
                _categoryService.Update(category);
            }
            else
            {
                _categoryService.Create(category);
            }

            return AjaxForm().RedirectTo(@return);
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Delete(CategoryRowModel[] model)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry(_ =>
            {
                foreach (var obj in model)
                {
                    var cate = _categoryService.GetById(obj.Id);
                    _categoryService.Delete(cate);
                }
                data.ReloadPage = true;
            });

            return Json(data);
            //var ids = model.Select(x => x.Id).ToArray();
            //var categories = _categoryService.Query()
            //    .Where(x => ids.Contains(x.Id))
            //    .ToList();

            //foreach (var category in categories)
            //{
            //    if (category.Children != null)
            //    {
            //        foreach (var child in category.Children)
            //        {
            //            child.Parent = null;
            //        }
            //    }
            //}

            //foreach (var category in categories)
            //{
            //    _categoryService.Delete(category);
            //}

            //return AjaxForm().ReloadPage();
        }
    }
}