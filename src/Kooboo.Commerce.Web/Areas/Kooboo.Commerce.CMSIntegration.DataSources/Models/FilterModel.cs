using Kooboo.Commerce.CMSIntegration.DataSources.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.Commerce.CMSIntegration.DataSources.Models
{
    public class FilterModel
    {
        public string Name { get; set; }

        public IList<FilterParameterModel> Parameters { get; set; }

        public FilterModel()
        {
            Parameters = new List<FilterParameterModel>();
        }

        public FilterModel(FilterDefinition filter)
        {
            Name = filter.Name;
            Parameters = filter.Parameters.Select(p => new FilterParameterModel
            {
                Name = p.Name
            })
            .ToList();
        }
    }
    public class FilterParameterModel
    {
        public string Name { get; set; }
    }
}