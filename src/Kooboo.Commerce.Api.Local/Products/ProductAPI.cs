using System.Linq;
using Kooboo.Commerce.Api.Products;

namespace Kooboo.Commerce.Api.Local.Products
{
    public class ProductApi : LocalCommerceQuery<Product, Kooboo.Commerce.Products.Product>, IProductApi
    {
        public ProductApi(LocalApiContext context)
            : base(context)
        {
        }

        protected override IQueryable<Commerce.Products.Product> CreateQuery()
        {
            return Context.Services.Products.Query();
        }

        protected override IQueryable<Commerce.Products.Product> OrderByDefault(IQueryable<Commerce.Products.Product> query)
        {
            return query.OrderByDescending(o => o.Id);
        }

        public IProductQuery ById(int id)
        {
            Query = Query.Where(o => o.Id == id);
            return this;
        }

        public IProductQuery ByCategoryId(int categoryId)
        {
            Query = Query.Where(o => o.Categories.Any(c => c.CategoryId == categoryId));
            return this;
        }

        public IProductQuery ByName(string name)
        {
            Query = Query.Where(o => o.Name == name);
            return this;
        }

        public IProductQuery ByProductTypeId(int productTypeId)
        {
            Query = Query.Where(o => o.ProductTypeId == productTypeId);
            return this;
        }

        public IProductQuery ByBrandId(int brandId)
        {
            Query = Query.Where(o => o.BrandId == brandId);
            return this;
        }

        public IProductQuery ByCustomField(string customFieldName, string fieldValue)
        {
            Query = Query.Where(o => o.CustomFields.Any(f => f.CustomField.Name == customFieldName && f.FieldValue == fieldValue));
            return this;
        }

        public IProductQuery ByVariantField(string variantName, string variantValue)
        {
            Query = Query.Where(o => o.Variants.Any(p => p.VariantFields.Any(v => v.CustomField.Name == variantName && v.FieldValue == variantValue)));
            return this;
        }
    }
}
