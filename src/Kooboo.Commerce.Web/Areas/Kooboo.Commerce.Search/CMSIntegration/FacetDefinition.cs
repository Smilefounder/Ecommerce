using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.CMSIntegration
{
    public class FacetDefinition
    {
        public string Name { get; set; }

        public FacetMode Mode { get; set; }

        public static IList<FacetDefinition> GetFacetDefinitions(IEnumerable<ProductType> productTypes)
        {
            var facets = new List<FacetDefinition>
            {
                new FacetDefinition { Name = "Brand", Mode = FacetMode.Default },
                new FacetDefinition { Name = "Category", Mode = FacetMode.Default },
                new FacetDefinition { Name = "Price", Mode = FacetMode.Ranges }
            };

            foreach (var productType in productTypes)
            {
                foreach (var fieldDef in productType.VariantFieldDefinitions)
                {
                    if (!facets.Any(f => f.Name == fieldDef.Name))
                    {
                        facets.Add(new FacetDefinition
                        {
                            Name = fieldDef.Name,
                            Mode = FacetMode.Default
                        });
                    }
                }
            }

            return facets;
        }
    }
}