using Kooboo.Commerce.Products;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Commerce.Search.Controllers
{
    public class TestController : Controller
    {
        public void Index()
        {
            var indexer = DocumentIndexers.GetIndexer("Vitaminstore", CultureInfo.InvariantCulture, typeof(Product));

            var serviceFactory = new DefaultServiceFactory();
            var fields = new List<string>();
            var productTypes = serviceFactory.ProductTypes.Query().ToList();
            foreach (var type in productTypes)
            {
                foreach (var field in type.VariantFieldDefinitions)
                {
                    fields.Add(field.Name);
                }
            }

            var facets = indexer.Facets(new MatchAllDocsQuery(), fields.ToArray());
            foreach (var facet in facets)
            {
                Response.Write("<h4>" + facet.FieldName + "</h4>");
                foreach (var item in facet.Items)
                {
                    Response.Write(item.Name + "(" + item.Count + ")" + ", ");
                }
            }
        }
    }
}
