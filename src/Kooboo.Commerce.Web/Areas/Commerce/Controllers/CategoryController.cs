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
using Kooboo.Commerce.Web.Framework.Mvc;
using Kooboo.Globalization;
using AutoMapper;

namespace Kooboo.Commerce.Web.Areas.Commerce.Controllers
{
    public class CategoryController : CommerceController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ActionResult Index(int page = 1, int pageSize = 50)
        {
            var categories = _categoryService.Query().Where(o => o.Parent == null)
                .OrderByDescending(x => x.Id)
                .Select(x => new CategoryRowModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ChildrenCount = x.Children.Count
                })
                .Paginate(page - 1, pageSize)
                .ToPagedList();

            return View(categories);
        }

        public ActionResult Children(int parentId)
        {
            var children = _categoryService.Query().Where(o => o.Parent.Id == parentId)
                .Select(x => new CategoryRowModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ChildrenCount = x.Children.Count
                })
                .ToArray();
            return JsonNet(children);
        }

        public ActionResult Create(int? parentId)
        {
            var model = new CategoryEditorModel
            {
                ParentId = parentId
            };

            PrepareEditor(model);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var category = _categoryService.GetById(id);
            var model = Mapper.Map<Category, CategoryEditorModel>(category);
            PrepareEditor(model);

            return View(model);
        }

        private void PrepareEditor(CategoryEditorModel model)
        {
            ViewBag.ParentPath = "None".Localize();
            if (model.ParentId != null)
            {
                var parent = CategoryTree.Get(CurrentInstance.Name).Find(model.ParentId.Value);
                ViewBag.ParentPath = String.Join(" >> ", parent.PathFromRoot().Select(c => c.Name));
            }
        }

        [HttpPost, HandleAjaxFormError]
        public ActionResult Save(CategoryEditorModel model, string @return)
        {
            Category category = null;

            if (model.Id > 0)
            {
                category = _categoryService.GetById(model.Id);
            }
            else
            {
                category = new Category();
            }

            category.Name = model.Name;
            category.Description = model.Description;
            category.Photo = model.Photo;

            if (model.ParentId != null)
            {
                category.Parent = _categoryService.GetById(model.ParentId.Value);
            }

            category.CustomFields.Clear();

            foreach (var field in model.CustomFields)
            {
                category.CustomFields.Add(new CategoryCustomField(field.Name, field.Value));
            }

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