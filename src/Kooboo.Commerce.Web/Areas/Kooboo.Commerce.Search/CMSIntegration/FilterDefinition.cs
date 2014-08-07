using Kooboo.Commerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.Search.CMSIntegration
{
    public class FilterDefinition
    {
        public string Field { get; set; }

        public bool SupportRangeFiltering { get; set; }

        public static IList<FilterDefinition> GetFilterDefinitions(IEnumerable<ProductType> productTypes)
        {
            var filters = new List<FilterDefinition>
            {
                new FilterDefinition { Field = "Brand" },
                new FilterDefinition { Field = "Category" },
                new FilterDefinition { Field = "MinPrice", SupportRangeFiltering = true }
            };

            foreach (var productType in productTypes)
            {
                foreach (var fieldDef in productType.VariantFieldDefinitions)
                {
                    if (!filters.Any(f => f.Field == fieldDef.Name))
                    {
                        filters.Add(new FilterDefinition
                        {
                            Field = fieldDef.Name
                        });
                    }
                }
            }

            return filters;
        }
    }
}