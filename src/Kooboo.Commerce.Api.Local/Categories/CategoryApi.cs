using System.Linq;
using Kooboo.Commerce.Api.Categories;

namespace Kooboo.Commerce.Api.Local.Categories
{
    public class CategoryApi : LocalCommerceQuery<Category, Kooboo.Commerce.Categories.Category>, ICategoryApi
    {
        public CategoryApi(LocalApiContext context)
            : base(context)
        {
        }

        protected override IQueryable<Commerce.Categories.Category> CreateQuery()
        {
            return Context.Services.Categories.Query();
        }

        protected override IQueryable<Commerce.Categories.Category> OrderByDefault(IQueryable<Commerce.Categories.Category> query)
        {
            return query.OrderBy(o => o.Id);
        }

        public ICategoryQuery ById(int id)
        {
            Query = Query.Where(o => o.Id == id).OrderBy(o => o.Name);
            return this;
        }

        public ICategoryQuery ByName(string name)
        {
            Query = Query.Where(o => o.Name == name);
            return this;
        }

        public ICategoryQuery Published(bool published)
        {
            Query = Query.Where(o => o.Published == published);
            return this;
        }

        public ICategoryQuery ByParentId(int? parentId)
        {
            if (parentId.HasValue)
                Query = Query.Where(o => o.Parent.Id == parentId.Value);
            else
                Query = Query.Where(o => o.Parent == null);
            return this;
        }

        public ICategoryQuery ByCustomField(string fieldName, string fieldValue)
        {
            Query = Query.Where(c => c.CustomFields.Any(f => f.Name == fieldName && f.Value == fieldValue));
            return this;
        }
    }
}
