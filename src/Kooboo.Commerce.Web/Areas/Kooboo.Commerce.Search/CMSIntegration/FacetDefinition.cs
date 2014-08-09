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

        public string Field { get; set; }

        public FacetMode Mode { get; set; }

        public static IList<FacetDefinition> GetFacetDefinitions(IEnumerable<ProductType> productTypes)
        {
            var facets = new List<FacetDefinition>
            {
                new FacetDefinition { Name = "Brand", Field = "Brand", Mode = FacetMode.Default },
                new FacetDefinition { Name = "Category", Field = "Categories", Mode = FacetMode.Default },
                new FacetDefinition { Name = "Price", Field = "Prices", Mode = FacetMode.Ranges }
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
                            Field = ModelConverter.GetDictionaryFieldName("VariantFieldValues", fieldDef.Name),
                            Mode = FacetMode.Default
                        });
                    }
                }
            }

            return facets;
        }
    }
}