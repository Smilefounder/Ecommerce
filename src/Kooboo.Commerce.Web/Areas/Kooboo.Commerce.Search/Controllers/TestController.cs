using Kooboo.Commerce.Products;
using Kooboo.Commerce.Search.Facets;
using Lucene.Net.Index;
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
            var indexer = DocumentIndexers.GetLiveIndexer("Vitaminstore", typeof(Product), CultureInfo.InvariantCulture);
            var doc = indexer.Search(new MatchAllDocsQuery(), Int32.MaxValue);
            Response.Write(doc.TotalHits);

            var results = indexer.Facets(new MatchAllDocsQuery(), new Facet[] {
                new Facet { Name = "Brand" },
                new Facet { Name = "Price", Ranges = new List<FacetRange>
                {
                    FacetRange.Parse("[0 TO 5000}", "[0 TO 5000}"),
                    FacetRange.Parse("[5000 TO 10000}", "[5000 TO 10000}"),
                    FacetRange.Parse("[10000 TO NULL]", "[10000 TO NULL]")
                }}
            });

            foreach (var result in results)
            {
                Response.Write("<h5>" + result.Name+ "</h5>");
                foreach (var value in result.Values)
                {
                    Response.Write(value.Term + "(" + value.Hits + "), ");
                }
            }
        }
    }
}
